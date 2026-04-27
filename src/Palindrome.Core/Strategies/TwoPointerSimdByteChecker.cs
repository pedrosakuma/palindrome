using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Byte-narrowed sibling of <see cref="TwoPointerSimdChecker"/>. Two
/// adjacent <c>ushort</c> loads are packed into a single byte vector via
/// <c>vpackuswb</c>, doubling the effective lane count: V256 processes
/// 32 chars per side per iteration (vs 16 in the ushort version) and
/// V128 processes 16 (vs 8).
///
/// <para>
/// The narrowing is lossless because callers establish that the input
/// is ASCII (high byte always 0), so saturating-pack is just a free
/// halving of the data type. SIMD compares in byte space have the same
/// throughput as in ushort space but cover twice as many lanes — every
/// pshufb/cmp/extractmsb instruction does double the work.
/// </para>
///
/// <para>
/// Trade-offs vs the ushort version:
/// <list type="bullet">
///   <item>+1 load and +1 pack (or pack+permute on V256) per side per
///         iteration — amortised across 2× the lanes.</item>
///   <item>Reverse on the right side now operates on bytes (V128: a
///         single 16-byte pshufb; V256: lane-swap + in-lane pshufb).</item>
///   <item>Skip-on-symbol granularity is in chars regardless, so the
///         net advance per iteration roughly doubles when input is
///         clean and stays the same when it is dirty.</item>
/// </list>
/// </para>
/// </summary>
[SkipLocalsInit]
public sealed class TwoPointerSimdByteChecker : IPalindromeChecker
{
    public string Name => "TwoPointer SIMD byte (no normalize)";

    private const int W128B = 16; // chars per side per V128 iter
    private const int W256B = 32; // chars per side per V256 iter

    private static readonly Vector128<byte> ReverseBytes128 = Vector128.Create(
        (byte)15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0);

    // pshufb is in-lane on AVX2; we put the same byte-reverse pattern in
    // both 128-bit halves and combine with a permute2x128 lane swap.
    private static readonly Vector256<byte> ReverseBytes256 = Vector256.Create(
        (byte)15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0,
              15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0);

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        var data = MemoryMarshal.Cast<char, ushort>(text);
        ref ushort baseRef = ref MemoryMarshal.GetReference(data);

        int left = 0;
        int right = data.Length - 1;

        // V256-byte main loop — 32 chars/side/iter.
        if (Vector256.IsHardwareAccelerated && Avx2.IsSupported)
        {
            var z0 = Vector256.Create((byte)'0');
            var z9 = Vector256.Create((byte)'9');
            var za = Vector256.Create((byte)'a');
            var zz = Vector256.Create((byte)'z');
            var zCase = Vector256.Create((byte)0x20);

            while (right - left + 1 >= 2 * W256B)
            {
                // Left: pack 32 chars [left .. left+32) into one byte vector.
                var lLo = Vector256.LoadUnsafe(ref baseRef, (nuint)left).AsInt16();
                var lHi = Vector256.LoadUnsafe(ref baseRef, (nuint)(left + 16)).AsInt16();
                var lvB = NarrowAsciiInOrder256(lLo, lHi);

                // Right: pack [right-31 .. right+1) then byte-reverse.
                int rStart = right - W256B + 1;
                var rLo = Vector256.LoadUnsafe(ref baseRef, (nuint)rStart).AsInt16();
                var rHi = Vector256.LoadUnsafe(ref baseRef, (nuint)(rStart + 16)).AsInt16();
                var rvBraw = NarrowAsciiInOrder256(rLo, rHi);
                var rvB = Reverse256B(rvBraw);

                uint maskL = AlnumMask256B(lvB, z0, z9, za, zz, zCase);
                uint maskR = AlnumMask256B(rvB, z0, z9, za, zz, zCase);

                // 32-bit masks: BitOperations.TrailingZeroCount(0u) == 32
                // by spec, so no sentinel bit is needed for the cap.
                int runL = BitOperations.TrailingZeroCount(~maskL);
                int runR = BitOperations.TrailingZeroCount(~maskR);
                int common = Math.Min(runL, runR);

                if (common == 0)
                {
                    int skipL = BitOperations.TrailingZeroCount(maskL);
                    int skipR = BitOperations.TrailingZeroCount(maskR);
                    left += skipL;
                    right -= skipR;
                    continue;
                }

                var eq = Vector256.Equals(lvB | zCase, rvB | zCase);
                uint eqMask = eq.ExtractMostSignificantBits();
                uint required = common == 32 ? 0xFFFFFFFFu : (1u << common) - 1u;
                if ((eqMask & required) != required) return false;

                left += common;
                right -= common;
            }
        }

        // V128-byte fallback — 16 chars/side/iter. Picks up any segment
        // shorter than 64 chars (or runs as the main loop when AVX2 is
        // absent but SSE2/SSSE3 are present).
        if (Vector128.IsHardwareAccelerated && Sse2.IsSupported && Ssse3.IsSupported)
        {
            var z0 = Vector128.Create((byte)'0');
            var z9 = Vector128.Create((byte)'9');
            var za = Vector128.Create((byte)'a');
            var zz = Vector128.Create((byte)'z');
            var zCase = Vector128.Create((byte)0x20);

            while (right - left + 1 >= 2 * W128B)
            {
                // Left: 16 chars [left .. left+16). V128 pack has no
                // lane-crossing problem (single 128-bit lane) so the
                // packed result is already in the correct order.
                var lLo = Vector128.LoadUnsafe(ref baseRef, (nuint)left).AsInt16();
                var lHi = Vector128.LoadUnsafe(ref baseRef, (nuint)(left + 8)).AsInt16();
                var lvB = Sse2.PackUnsignedSaturate(lLo, lHi);

                int rStart = right - W128B + 1;
                var rLo = Vector128.LoadUnsafe(ref baseRef, (nuint)rStart).AsInt16();
                var rHi = Vector128.LoadUnsafe(ref baseRef, (nuint)(rStart + 8)).AsInt16();
                var rvBraw = Sse2.PackUnsignedSaturate(rLo, rHi);
                var rvB = Ssse3.Shuffle(rvBraw, ReverseBytes128);

                uint maskL = AlnumMask128B(lvB, z0, z9, za, zz, zCase);
                uint maskR = AlnumMask128B(rvB, z0, z9, za, zz, zCase);

                int runL = BitOperations.TrailingZeroCount((~maskL) | 0x10000u);
                int runR = BitOperations.TrailingZeroCount((~maskR) | 0x10000u);
                int common = Math.Min(runL, runR);

                if (common == 0)
                {
                    int skipL = BitOperations.TrailingZeroCount(maskL | 0x10000u);
                    int skipR = BitOperations.TrailingZeroCount(maskR | 0x10000u);
                    left += skipL;
                    right -= skipR;
                    continue;
                }

                var eq = Vector128.Equals(lvB | zCase, rvB | zCase);
                uint eqMask = eq.ExtractMostSignificantBits();
                uint required = common == 16 ? 0xFFFFu : (1u << common) - 1u;
                if ((eqMask & required) != required) return false;

                left += common;
                right -= common;
            }
        }

        // Scalar tail.
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

    /// <summary>
    /// Packs two <see cref="Vector256{Int16}"/> lanes of 16-bit ASCII
    /// values into a single <see cref="Vector256{Byte}"/> in left-to-right
    /// memory order. <c>vpackuswb</c> is in-lane: it interleaves the two
    /// inputs per 128-bit lane, so we must follow it with a
    /// <c>vpermq 0xD8</c> to restore sequential order across the 256-bit
    /// vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector256<byte> NarrowAsciiInOrder256(Vector256<short> lo, Vector256<short> hi)
    {
        var packed = Avx2.PackUnsignedSaturate(lo, hi);
        // 0xD8 = 0b11_01_10_00: qword permutation 0,2,1,3 — swaps the two
        // middle 64-bit chunks to fix the in-lane interleave.
        return Avx2.Permute4x64(packed.AsInt64(), 0xD8).AsByte();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector256<byte> Reverse256B(Vector256<byte> v)
    {
        // Swap the two 16-byte lanes, then byte-reverse within each lane.
        var swapped = Avx2.Permute2x128(v, v, 0x01);
        return Avx2.Shuffle(swapped, ReverseBytes256);
    }

    /// <summary>
    /// 32-bit alnum mask for a 256-bit byte window (one bit per byte lane).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AlnumMask256B(
        Vector256<byte> v,
        Vector256<byte> z0, Vector256<byte> z9,
        Vector256<byte> za, Vector256<byte> zz,
        Vector256<byte> zCase)
    {
        var isDigit = Vector256.GreaterThanOrEqual(v, z0) & Vector256.LessThanOrEqual(v, z9);
        var folded = v | zCase;
        var isAlpha = Vector256.GreaterThanOrEqual(folded, za) & Vector256.LessThanOrEqual(folded, zz);
        return (isDigit | isAlpha).ExtractMostSignificantBits();
    }

    /// <summary>16-bit alnum mask for a 128-bit byte window.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AlnumMask128B(
        Vector128<byte> v,
        Vector128<byte> z0, Vector128<byte> z9,
        Vector128<byte> za, Vector128<byte> zz,
        Vector128<byte> zCase)
    {
        var isDigit = Vector128.GreaterThanOrEqual(v, z0) & Vector128.LessThanOrEqual(v, z9);
        var folded = v | zCase;
        var isAlpha = Vector128.GreaterThanOrEqual(folded, za) & Vector128.LessThanOrEqual(folded, zz);
        return (isDigit | isAlpha).ExtractMostSignificantBits();
    }
}
