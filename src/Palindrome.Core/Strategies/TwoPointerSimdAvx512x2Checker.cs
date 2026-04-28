using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Double-pumped AVX-512 byte-narrowed two-pointer scan: 128 chars per side
/// per iteration (two 512-bit byte vectors live concurrently per side).
///
/// <para>
/// The hypothesis was that the single-pumped <see cref="TwoPointerSimdAvx512Checker"/>
/// was bound by the load → narrow → reverse → compare chain on long inputs,
/// so interleaving two independent chains per side should expose more MLP
/// and better hide L2 latency on inputs that spill out of L1 (≥ 32 KiB / side).
/// </para>
///
/// <para>
/// <b>Result: hypothesis was wrong.</b> Measured on Intel Xeon Platinum 8370C
/// (Ice Lake-SP, AVX-512 BW+VBMI), the double-pumped variant is consistently
/// slower than single-pumped at every input size — and both AVX-512 variants
/// lose to the V128 <see cref="TwoPointerSimdChecker"/> on this hardware:
/// </para>
///
/// <code>
/// Length  | V128 (no narrow) | AVX-512 ×1 | AVX-512 ×2
///     64  |       11.5 ns    |   63.1 ns  |   67.1 ns
///   1024  |       861  ns    | 1002   ns  | 1016   ns
///  16384  |      14.1  µs    |   15.8 µs  |   17.4 µs
/// 262144  |      235   µs    |  256   µs  |  277   µs
/// </code>
///
/// <para>
/// Why double-pumping lost: (1) AVX-512 frequency throttling on Ice Lake-SP
/// — denser zmm ops per iteration deepen the down-clock; (2) L2 bandwidth
/// is the real bottleneck at large sizes, not chain latency, so adding more
/// loads in flight does nothing; (3) <c>vpermb</c> has 3-cycle latency but
/// throughput is one per cycle, so the single-pumped chain already fills
/// the port — doubling it just queues up.
/// </para>
///
/// <para>
/// Kept in the repo as a documented negative result: a faster ISA / wider
/// vectors / more MLP is not automatically faster. Always measure.
/// </para>
///
/// <para>
/// Same hard requirement as the single-pumped variant: <c>AVX-512BW</c> +
/// <c>AVX-512VBMI</c> (vpermb). Falls back to
/// <see cref="TwoPointerSimdByteChecker"/> on CPUs without VBMI.
/// </para>
/// </summary>
[SkipLocalsInit]
public sealed class TwoPointerSimdAvx512x2Checker : IPalindromeChecker
{
    public string Name => "TwoPointer SIMD AVX-512 x2 (no normalize)";

    private const int W = 64;        // chars per 512-bit byte vector
    private const int W2 = 2 * W;    // 128 chars per side per iteration

    private readonly TwoPointerSimdByteChecker _fallback = new();

    private static readonly Vector512<byte> ReverseBytes512 = Vector512.Create(
        (byte)63, 62, 61, 60, 59, 58, 57, 56,
              55, 54, 53, 52, 51, 50, 49, 48,
              47, 46, 45, 44, 43, 42, 41, 40,
              39, 38, 37, 36, 35, 34, 33, 32,
              31, 30, 29, 28, 27, 26, 25, 24,
              23, 22, 21, 20, 19, 18, 17, 16,
              15, 14, 13, 12, 11, 10,  9,  8,
               7,  6,  5,  4,  3,  2,  1,  0);

    private static readonly Vector512<long> RestorePackOrder = Vector512.Create(
        0L, 2L, 4L, 6L, 1L, 3L, 5L, 7L);

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        if (!Avx512BW.IsSupported || !Avx512Vbmi.IsSupported)
            return _fallback.IsPalindrome(text);

        var data = MemoryMarshal.Cast<char, ushort>(text);
        ref ushort baseRef = ref MemoryMarshal.GetReference(data);

        int left = 0;
        int right = data.Length - 1;

        var z0 = Vector512.Create((byte)'0');
        var z9 = Vector512.Create((byte)'9');
        var za = Vector512.Create((byte)'a');
        var zz = Vector512.Create((byte)'z');
        var zCase = Vector512.Create((byte)0x20);

        while (right - left + 1 >= 2 * W2)
        {
            // ---- Left side: 128 chars → two 64-byte vectors in order ----
            var lLo0 = Vector512.LoadUnsafe(ref baseRef, (nuint)left).AsInt16();
            var lHi0 = Vector512.LoadUnsafe(ref baseRef, (nuint)(left + 32)).AsInt16();
            var lLo1 = Vector512.LoadUnsafe(ref baseRef, (nuint)(left + 64)).AsInt16();
            var lHi1 = Vector512.LoadUnsafe(ref baseRef, (nuint)(left + 96)).AsInt16();

            var lvB0 = NarrowAsciiInOrder512(lLo0, lHi0); // chars [left .. left+63]
            var lvB1 = NarrowAsciiInOrder512(lLo1, lHi1); // chars [left+64 .. left+127]

            // ---- Right side: 128 chars, reversed so positions align ----
            // Reading order in memory: [rStart .. rStart+127] where
            //   rStart = right - W2 + 1
            // After reverse, lvB0 must be compared with the reverse of the
            // *last* 64 bytes (positions [right-63 .. right]) and lvB1 with
            // the reverse of the *first* 64 bytes (positions [rStart .. rStart+63]).
            int rStart = right - W2 + 1;
            var rLo0 = Vector512.LoadUnsafe(ref baseRef, (nuint)rStart).AsInt16();
            var rHi0 = Vector512.LoadUnsafe(ref baseRef, (nuint)(rStart + 32)).AsInt16();
            var rLo1 = Vector512.LoadUnsafe(ref baseRef, (nuint)(rStart + 64)).AsInt16();
            var rHi1 = Vector512.LoadUnsafe(ref baseRef, (nuint)(rStart + 96)).AsInt16();

            var rvBraw0 = NarrowAsciiInOrder512(rLo0, rHi0); // memory order, low half of right window
            var rvBraw1 = NarrowAsciiInOrder512(rLo1, rHi1); // memory order, high half of right window

            // After byte-reverse, swap pairing so high half of right matches
            // the low half of left (i.e. the inner-most chars on each side).
            var rvB0 = Avx512Vbmi.PermuteVar64x8(rvBraw1, ReverseBytes512); // pairs with lvB0
            var rvB1 = Avx512Vbmi.PermuteVar64x8(rvBraw0, ReverseBytes512); // pairs with lvB1

            ulong maskL0 = AlnumMask512(lvB0, z0, z9, za, zz, zCase);
            ulong maskR0 = AlnumMask512(rvB0, z0, z9, za, zz, zCase);

            // Common alnum prefix in the *first* 64-lane window. If it's not
            // a full 64, every char beyond that is a symbol or a mismatch we
            // need to handle scalar-ish, so fall through to the slow path
            // that treats this iteration as a single 64-wide step. Keeping
            // the fast path branch-light is what makes double-pumping pay.
            int runL0 = BitOperations.TrailingZeroCount(~maskL0);
            int runR0 = BitOperations.TrailingZeroCount(~maskR0);
            int common0 = Math.Min(runL0, runR0);

            if (common0 < W)
            {
                // Symbol or partial-alnum window: handle with the same logic
                // as the single-pumped variant on just the first 64-lane
                // window, then loop. We deliberately do NOT use the second
                // window's loads here — they'll be re-issued next iteration
                // (cheap: still in L1).
                if (common0 == 0)
                {
                    int skipL = BitOperations.TrailingZeroCount(maskL0);
                    int skipR = BitOperations.TrailingZeroCount(maskR0);
                    left += skipL;
                    right -= skipR;
                    continue;
                }

                var eqA = Vector512.Equals(lvB0 | zCase, rvB0 | zCase);
                ulong eqMaskA = eqA.ExtractMostSignificantBits();
                ulong reqA = (1ul << common0) - 1ul;
                if ((eqMaskA & reqA) != reqA) return false;

                left += common0;
                right -= common0;
                continue;
            }

            // First window is fully alnum on both sides → check the second.
            ulong maskL1 = AlnumMask512(lvB1, z0, z9, za, zz, zCase);
            ulong maskR1 = AlnumMask512(rvB1, z0, z9, za, zz, zCase);
            int runL1 = BitOperations.TrailingZeroCount(~maskL1);
            int runR1 = BitOperations.TrailingZeroCount(~maskR1);
            int common1 = Math.Min(runL1, runR1);

            // Always need the first window's eq result.
            var eq0 = Vector512.Equals(lvB0 | zCase, rvB0 | zCase);
            ulong eqMask0 = eq0.ExtractMostSignificantBits();
            if (eqMask0 != ulong.MaxValue) return false;

            if (common1 == 64)
            {
                // 128 alnum on both sides → compare the second window in full.
                var eq1 = Vector512.Equals(lvB1 | zCase, rvB1 | zCase);
                ulong eqMask1 = eq1.ExtractMostSignificantBits();
                if (eqMask1 != ulong.MaxValue) return false;
                left += W2;
                right -= W2;
            }
            else
            {
                // Partial second window: compare only the alnum prefix.
                if (common1 > 0)
                {
                    var eq1 = Vector512.Equals(lvB1 | zCase, rvB1 | zCase);
                    ulong eqMask1 = eq1.ExtractMostSignificantBits();
                    ulong req1 = (1ul << common1) - 1ul;
                    if ((eqMask1 & req1) != req1) return false;
                }
                left += W + common1;
                right -= W + common1;
            }
        }

        // Tail: scalar two-pointer for remaining < 2 * W2 = 256 chars.
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
    private static Vector512<byte> NarrowAsciiInOrder512(Vector512<short> lo, Vector512<short> hi)
    {
        var packed = Avx512BW.PackUnsignedSaturate(lo, hi);
        return Avx512F.PermuteVar8x64(packed.AsInt64(), RestorePackOrder).AsByte();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AlnumMask512(
        Vector512<byte> v,
        Vector512<byte> z0, Vector512<byte> z9,
        Vector512<byte> za, Vector512<byte> zz,
        Vector512<byte> zCase)
    {
        var isDigit = Vector512.GreaterThanOrEqual(v, z0) & Vector512.LessThanOrEqual(v, z9);
        var folded = v | zCase;
        var isAlpha = Vector512.GreaterThanOrEqual(folded, za) & Vector512.LessThanOrEqual(folded, zz);
        return (isDigit | isAlpha).ExtractMostSignificantBits();
    }
}
