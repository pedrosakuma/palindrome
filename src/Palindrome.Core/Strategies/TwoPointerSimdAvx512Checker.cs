using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Palindrome.Core.Strategies;

/// <summary>
/// AVX-512 byte-narrowed two-pointer scan. Processes 64 chars per side per
/// iteration (vs 32 in <see cref="TwoPointerSimdByteChecker"/>).
///
/// <para>
/// Requires <c>AVX-512BW</c> (byte/word ops) and <c>AVX-512VBMI</c> for the
/// single-instruction cross-lane byte reverse via <c>vpermb</c>. On any CPU
/// missing either feature (e.g. Zen 3, pre-Ice-Lake Intel) this checker
/// transparently delegates to <see cref="TwoPointerSimdByteChecker"/>.
/// </para>
///
/// <para>
/// Wins over the AVX2 byte version come from:
/// <list type="bullet">
///   <item>Twice the lane count per iteration (64 vs 32 bytes).</item>
///   <item><c>vpermb</c> performs the cross-lane reverse in a single
///         instruction — vs AVX2's <c>vpermq</c> + <c>vpshufb</c>
///         sequence — eliminating one cross-lane bubble per iteration.</item>
///   <item><c>vpermq</c> after <c>vpackuswb</c> restores sequential order
///         across all four 128-bit lanes in one instruction (AVX2 only
///         had two lanes to fix).</item>
/// </list>
/// </para>
/// </summary>
[SkipLocalsInit]
public sealed class TwoPointerSimdAvx512Checker : IPalindromeChecker
{
    public string Name => "TwoPointer SIMD AVX-512 (no normalize)";

    private const int W = 64; // chars per side per iteration

    private readonly TwoPointerSimdByteChecker _fallback = new();

    /// <summary>
    /// Byte-reverse pattern for <c>vpermb</c>: lane <c>i</c> ← lane <c>63-i</c>.
    /// </summary>
    private static readonly Vector512<byte> ReverseBytes512 = Vector512.Create(
        (byte)63, 62, 61, 60, 59, 58, 57, 56,
              55, 54, 53, 52, 51, 50, 49, 48,
              47, 46, 45, 44, 43, 42, 41, 40,
              39, 38, 37, 36, 35, 34, 33, 32,
              31, 30, 29, 28, 27, 26, 25, 24,
              23, 22, 21, 20, 19, 18, 17, 16,
              15, 14, 13, 12, 11, 10,  9,  8,
               7,  6,  5,  4,  3,  2,  1,  0);

    /// <summary>
    /// Qword permutation that undoes <c>vpackuswb</c>'s in-lane interleave
    /// for two 512-bit inputs. After the pack, source qwords sit at
    /// positions [0,2,4,6,1,3,5,7]; we permute them back to sequential
    /// order via <c>vpermq</c>.
    /// </summary>
    private static readonly Vector512<long> RestorePackOrder = Vector512.Create(
        0L, 2L, 4L, 6L, 1L, 3L, 5L, 7L);

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        // Hard requirement: AVX-512BW for byte compares, AVX-512VBMI for
        // vpermb. If either is missing, fall back to the AVX2 byte path —
        // identical semantics, smaller vectors.
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

        while (right - left + 1 >= 2 * W)
        {
            // Left side: load 64 chars (two 512-bit ushort vectors) and
            // narrow to a single 512-bit byte vector in memory order.
            var lLo = Vector512.LoadUnsafe(ref baseRef, (nuint)left).AsInt16();
            var lHi = Vector512.LoadUnsafe(ref baseRef, (nuint)(left + 32)).AsInt16();
            var lvB = NarrowAsciiInOrder512(lLo, lHi);

            // Right side: same narrow, then byte-reverse with one vpermb.
            int rStart = right - W + 1;
            var rLo = Vector512.LoadUnsafe(ref baseRef, (nuint)rStart).AsInt16();
            var rHi = Vector512.LoadUnsafe(ref baseRef, (nuint)(rStart + 32)).AsInt16();
            var rvBraw = NarrowAsciiInOrder512(rLo, rHi);
            var rvB = Avx512Vbmi.PermuteVar64x8(rvBraw, ReverseBytes512);

            ulong maskL = AlnumMask512(lvB, z0, z9, za, zz, zCase);
            ulong maskR = AlnumMask512(rvB, z0, z9, za, zz, zCase);

            // 64-bit masks: BitOperations.TrailingZeroCount(0ul) == 64,
            // which equals W — so an all-alnum side naturally caps at W
            // without any sentinel bit.
            int runL = BitOperations.TrailingZeroCount(~maskL);
            int runR = BitOperations.TrailingZeroCount(~maskR);
            int common = Math.Min(runL, runR);

            if (common == 0)
            {
                // Symbol at lane 0 of one side — skip every leading
                // symbol on each side. TrailingZeroCount(0ul) == 64 here
                // means "all symbols, advance by W"; that is exactly
                // what we want.
                int skipL = BitOperations.TrailingZeroCount(maskL);
                int skipR = BitOperations.TrailingZeroCount(maskR);
                left += skipL;
                right -= skipR;
                continue;
            }

            var eq = Vector512.Equals(lvB | zCase, rvB | zCase);
            ulong eqMask = eq.ExtractMostSignificantBits();
            // (1ul << 64) is undefined in C# (acts as 1ul << 0); special-
            // case the full mask explicitly.
            ulong required = common == 64 ? ulong.MaxValue : (1ul << common) - 1ul;
            if ((eqMask & required) != required) return false;

            left += common;
            right -= common;
        }

        // Tail: scalar two-pointer is fine — at most 2*W-1 = 127 chars.
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
    /// Packs two 512-bit ushort vectors into one 512-bit byte vector in
    /// left-to-right memory order. <c>vpackuswb</c> is in-lane (per
    /// 128-bit lane) on AVX-512 just as on AVX2, so we follow with a
    /// <c>vpermq</c> over qword positions [0,2,4,6,1,3,5,7] to restore
    /// sequential order.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector512<byte> NarrowAsciiInOrder512(Vector512<short> lo, Vector512<short> hi)
    {
        var packed = Avx512BW.PackUnsignedSaturate(lo, hi);
        return Avx512F.PermuteVar8x64(packed.AsInt64(), RestorePackOrder).AsByte();
    }

    /// <summary>64-bit alnum mask, one bit per byte lane.</summary>
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
