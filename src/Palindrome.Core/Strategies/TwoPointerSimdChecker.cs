using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Palindrome.Core.Strategies;

/// <summary>
/// SIMD-accelerated in-place two-pointer scan. No allocation, no separate
/// normalization pass.
///
/// Each iteration:
///   1. Load W chars from the left and W chars from the right (reversed
///      so positions align).
///   2. Compute an "is-alphanumeric" mask for each side. The case-folded
///      range check (<c>v | 0x20</c>) covers both 'A'-'Z' and 'a'-'z' in
///      one compare.
///   3. Use <c>ExtractMostSignificantBits</c> + <c>TrailingZeroCount</c>
///      to find the longest *contiguous run of alnum lanes from the start*
///      on each side. Let <c>k = min(runL, runR)</c>.
///   4. If <c>k &gt; 0</c>: case-fold + masked equality check on those
///      <c>k</c> lanes; advance both pointers by <c>k</c>.
///   5. If <c>k == 0</c>: a symbol is sitting at one of the pointers, do
///      a single scalar step that skips it (or compares the pair).
///
/// On dirty input (~7% symbol density) every iteration advances by the
/// expected gap between symbols (~14 chars) instead of 1, so the SIMD
/// path stays hot even when the previous "all-or-nothing" version would
/// have fallen through to scalar 90% of the time.
/// </summary>
[SkipLocalsInit]
public sealed class TwoPointerSimdChecker : IPalindromeChecker
{
    public string Name => "TwoPointer SIMD (no normalize)";

    private const int W128 = 8;
    private const int W256 = 16;

    private static readonly Vector128<byte> ReverseBytes128 = Vector128.Create(
        (byte)14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

    private static readonly Vector256<byte> ReverseBytes256 = Vector256.Create(
        (byte)14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1,
                14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        var data = MemoryMarshal.Cast<char, ushort>(text);
        ref ushort baseRef = ref MemoryMarshal.GetReference(data);

        int left = 0;
        int right = data.Length - 1;

        if (Vector256.IsHardwareAccelerated && Avx2.IsSupported)
        {
            var z0 = Vector256.Create((ushort)'0');
            var z9 = Vector256.Create((ushort)'9');
            var za = Vector256.Create((ushort)'a');
            var zz = Vector256.Create((ushort)'z');
            var zCase = Vector256.Create((ushort)0x20);

            while (right - left + 1 >= 2 * W256)
            {
                var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
                var rvRaw = Vector256.LoadUnsafe(ref baseRef, (nuint)(right - W256 + 1));
                var rv = Reverse256(rvRaw);

                // ExtractMostSignificantBits on a Vector256<ushort> gives
                // 16 bits — one per lane (MSB of each ushort). After the
                // alnum compare each lane is 0xFFFF or 0x0000.
                uint maskL = AlnumMask256(lv, z0, z9, za, zz, zCase);
                uint maskR = AlnumMask256(rv, z0, z9, za, zz, zCase);

                // Trailing 1-count = TrailingZeroCount of the inverted
                // mask. OR with bit-16 sentinel to cap the result at 16
                // when all lanes are alnum.
                int runL = BitOperations.TrailingZeroCount((~maskL) | 0x10000u);
                int runR = BitOperations.TrailingZeroCount((~maskR) | 0x10000u);
                int common = Math.Min(runL, runR);

                if (common == 0)
                {
                    // A symbol sits at one of the pointers. Use the masks
                    // we already have to skip every leading symbol on each
                    // side in a single step. By construction at least one
                    // of skipL/skipR is > 0 (common == 0 means lane 0 is a
                    // symbol on at least one side, so its skip is >= 1).
                    int skipL = BitOperations.TrailingZeroCount(maskL | 0x10000u);
                    int skipR = BitOperations.TrailingZeroCount(maskR | 0x10000u);
                    left += skipL;
                    right -= skipR;
                    continue;
                }

                // Case-fold once and pull the equality bitmap.
                var eq = Vector256.Equals(lv | zCase, rv | zCase);
                uint eqMask = eq.ExtractMostSignificantBits();
                uint required = common == 16 ? 0xFFFFu : (1u << common) - 1u;
                if ((eqMask & required) != required) return false;

                left += common;
                right -= common;
            }
        }

        if (Vector128.IsHardwareAccelerated)
        {
            var z0 = Vector128.Create((ushort)'0');
            var z9 = Vector128.Create((ushort)'9');
            var za = Vector128.Create((ushort)'a');
            var zz = Vector128.Create((ushort)'z');
            var zCase = Vector128.Create((ushort)0x20);

            while (right - left + 1 >= 2 * W128)
            {
                var lv = Vector128.LoadUnsafe(ref baseRef, (nuint)left);
                var rvRaw = Vector128.LoadUnsafe(ref baseRef, (nuint)(right - W128 + 1));
                var rv = Reverse128(rvRaw);

                uint maskL = AlnumMask128(lv, z0, z9, za, zz, zCase);
                uint maskR = AlnumMask128(rv, z0, z9, za, zz, zCase);

                int runL = BitOperations.TrailingZeroCount((~maskL) | 0x100u);
                int runR = BitOperations.TrailingZeroCount((~maskR) | 0x100u);
                int common = Math.Min(runL, runR);

                if (common == 0)
                {
                    int skipL = BitOperations.TrailingZeroCount(maskL | 0x100u);
                    int skipR = BitOperations.TrailingZeroCount(maskR | 0x100u);
                    left += skipL;
                    right -= skipR;
                    continue;
                }

                var eq = Vector128.Equals(lv | zCase, rv | zCase);
                uint eqMask = eq.ExtractMostSignificantBits();
                uint required = common == 8 ? 0xFFu : (1u << common) - 1u;
                if ((eqMask & required) != required) return false;

                left += common;
                right -= common;
            }
        }

        // Scalar tail (or whole input when SIMD is unavailable).
        while (left < right)
        {
            char l = text[left];
            if (!AsciiHelpers.IsAlphanumeric(l)) { left++; continue; }
            char r = text[right];
            if (!AsciiHelpers.IsAlphanumeric(r)) { right--; continue; }
            if (AsciiHelpers.ToLowerAsciiInvariant(l) != AsciiHelpers.ToLowerAsciiInvariant(r))
                return false;
            left++;
            right--;
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ushort> Reverse128(Vector128<ushort> v)
    {
        if (Ssse3.IsSupported)
            return Ssse3.Shuffle(v.AsByte(), ReverseBytes128).AsUInt16();
        return Vector128.Shuffle(v, Vector128.Create((ushort)7, 6, 5, 4, 3, 2, 1, 0));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector256<ushort> Reverse256(Vector256<ushort> v)
    {
        var swapped = Avx2.Permute2x128(v.AsByte(), v.AsByte(), 0x01);
        return Avx2.Shuffle(swapped, ReverseBytes256).AsUInt16();
    }

    /// <summary>
    /// Returns a 16-bit mask where bit <c>i</c> is set when lane <c>i</c>
    /// is an ASCII digit or letter (case-folded range check on letters).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AlnumMask256(
        Vector256<ushort> v,
        Vector256<ushort> z0, Vector256<ushort> z9,
        Vector256<ushort> za, Vector256<ushort> zz,
        Vector256<ushort> zCase)
    {
        var isDigit = Vector256.GreaterThanOrEqual(v, z0) & Vector256.LessThanOrEqual(v, z9);
        var folded = v | zCase;
        var isAlpha = Vector256.GreaterThanOrEqual(folded, za) & Vector256.LessThanOrEqual(folded, zz);
        return (isDigit | isAlpha).ExtractMostSignificantBits();
    }

    /// <summary>8-bit alnum mask for a 128-bit window.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AlnumMask128(
        Vector128<ushort> v,
        Vector128<ushort> z0, Vector128<ushort> z9,
        Vector128<ushort> za, Vector128<ushort> zz,
        Vector128<ushort> zCase)
    {
        var isDigit = Vector128.GreaterThanOrEqual(v, z0) & Vector128.LessThanOrEqual(v, z9);
        var folded = v | zCase;
        var isAlpha = Vector128.GreaterThanOrEqual(folded, za) & Vector128.LessThanOrEqual(folded, zz);
        return (isDigit | isAlpha).ExtractMostSignificantBits();
    }
}
