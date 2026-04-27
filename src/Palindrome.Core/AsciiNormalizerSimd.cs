using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Palindrome.Core;

/// <summary>
/// Vectorized ASCII normalizer with a bulk fast-path: when an entire vector
/// is alphanumeric ASCII it is lower-cased and stored in one shot. Vectors
/// containing any non-alphanumeric character go through a SIMD <em>compress</em>
/// (pshufb-based gather indexed by the alnum bitmap), then a <c>popcount</c>
/// gives the number of valid lanes to advance. This replaces the per-lane
/// scalar gather that the previous implementation used.
///
/// <para>
/// The compress technique mirrors what AVX-512 VBMI2's <c>vpcompressw</c>
/// does in a single instruction. On AVX2-only hardware (Zen 3, Skylake)
/// we emulate it with a 256-entry × 16-byte lookup table of pshufb masks.
/// The table fits comfortably in L1 (4 KiB).
/// </para>
/// </summary>
[SkipLocalsInit]
public static class AsciiNormalizerSimd
{
    /// <summary>
    /// 256-entry table of pshufb shuffle masks. Indexed by an 8-bit
    /// "active lanes" bitmap (one bit per ushort lane). Each entry moves
    /// the active ushort lanes of a <see cref="Vector128{UInt16}"/> down
    /// to the lowest contiguous lanes, leaving the rest as garbage.
    /// </summary>
    private static readonly Vector128<byte>[] CompressShuffle128 = BuildCompressShuffle128();

    private static Vector128<byte>[] BuildCompressShuffle128()
    {
        var table = new Vector128<byte>[256];
        Span<byte> buf = stackalloc byte[16];
        for (int m = 0; m < 256; m++)
        {
            // pshufb treats any source-index byte with bit 7 set as "write 0".
            // Initialise to that sentinel so unused output lanes are deterministic.
            buf.Fill(0x80);
            int outLane = 0;
            for (int j = 0; j < 8; j++)
            {
                if (((m >> j) & 1) != 0)
                {
                    // ushort j occupies bytes [2j, 2j+1] of the source vector.
                    buf[outLane * 2]     = (byte)(j * 2);
                    buf[outLane * 2 + 1] = (byte)(j * 2 + 1);
                    outLane++;
                }
            }
            table[m] = Vector128.Create<byte>(buf);
        }
        return table;
    }

    /// <summary>
    /// Compresses the active lanes of <paramref name="v"/> (selected by
    /// the 8-bit <paramref name="mask"/>) into the low lanes of the
    /// returned vector. Requires SSSE3.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ushort> Compress128(Vector128<ushort> v, uint mask)
    {
        var shuf = CompressShuffle128[mask & 0xFF];
        return Ssse3.Shuffle(v.AsByte(), shuf).AsUInt16();
    }

    /// <summary>Normalize using <see cref="Vector256"/> (16 chars/iter).</summary>
    public static int Normalize256(ReadOnlySpan<char> source, Span<char> destination)
    {
        const int W = 16;
        var src = MemoryMarshal.Cast<char, ushort>(source);
        var dst = MemoryMarshal.Cast<char, ushort>(destination);

        if (!Vector256.IsHardwareAccelerated || !Ssse3.IsSupported || src.Length < W)
            return AsciiNormalizer.Normalize(source, destination);

        ref ushort srcRef = ref MemoryMarshal.GetReference(src);
        ref ushort dstRef = ref MemoryMarshal.GetReference(dst);

        var v0    = Vector256.Create((ushort)'0');
        var v9    = Vector256.Create((ushort)'9');
        var vUA   = Vector256.Create((ushort)'A');
        var vUZ   = Vector256.Create((ushort)'Z');
        var vCase = Vector256.Create((ushort)0x20);
        var vLA   = Vector256.Create((ushort)'a');
        var vLZ   = Vector256.Create((ushort)'z');

        int i = 0;
        int written = 0;
        int last = src.Length - W;

        while (i <= last)
        {
            var v = Vector256.LoadUnsafe(ref srcRef, (nuint)i);

            var isUpper =
                Vector256.GreaterThanOrEqual(v, vUA)
                & Vector256.LessThanOrEqual(v, vUZ);
            var lowered = v | (isUpper & vCase);

            var isDigit =
                Vector256.GreaterThanOrEqual(lowered, v0)
                & Vector256.LessThanOrEqual(lowered, v9);
            var isLetter =
                Vector256.GreaterThanOrEqual(lowered, vLA)
                & Vector256.LessThanOrEqual(lowered, vLZ);
            var isAlnum = isDigit | isLetter;

            uint mask = isAlnum.ExtractMostSignificantBits();
            if (mask == 0xFFFF)
            {
                // All-alnum fast path: bulk store the whole 256-bit vector.
                lowered.StoreUnsafe(ref dstRef, (nuint)written);
                written += W;
            }
            else
            {
                // Compress each 128-bit half independently. Each half writes
                // 16 bytes (8 ushorts) directly to dst at the current write
                // cursor; inactive trailing lanes are scratch and will be
                // overwritten by the next iteration. Safety: written ≤ i ≤
                // src.Length - W ≤ dst.Length - W, so writing 8 ushorts at
                // offset `written` (and again after advancing by ≤8) stays
                // in-bounds even for the final iteration.
                uint mLo = mask & 0xFFu;
                uint mHi = (mask >> 8) & 0xFFu;
                var compLo = Compress128(lowered.GetLower(), mLo);
                compLo.StoreUnsafe(ref dstRef, (nuint)written);
                written += BitOperations.PopCount(mLo);
                var compHi = Compress128(lowered.GetUpper(), mHi);
                compHi.StoreUnsafe(ref dstRef, (nuint)written);
                written += BitOperations.PopCount(mHi);
            }

            i += W;
        }

        for (; i < src.Length; i++)
        {
            char c = source[i];
            if (AsciiHelpers.IsAlphanumeric(c))
                destination[written++] = AsciiHelpers.ToLowerAsciiInvariant(c);
        }

        return written;
    }

    /// <summary>Normalize using <see cref="Vector128"/> (8 chars/iter).</summary>
    public static int Normalize128(ReadOnlySpan<char> source, Span<char> destination)
    {
        const int W = 8;
        var src = MemoryMarshal.Cast<char, ushort>(source);
        var dst = MemoryMarshal.Cast<char, ushort>(destination);

        if (!Vector128.IsHardwareAccelerated || !Ssse3.IsSupported || src.Length < W)
            return AsciiNormalizer.Normalize(source, destination);

        ref ushort srcRef = ref MemoryMarshal.GetReference(src);
        ref ushort dstRef = ref MemoryMarshal.GetReference(dst);

        var v0    = Vector128.Create((ushort)'0');
        var v9    = Vector128.Create((ushort)'9');
        var vUA   = Vector128.Create((ushort)'A');
        var vUZ   = Vector128.Create((ushort)'Z');
        var vCase = Vector128.Create((ushort)0x20);
        var vLA   = Vector128.Create((ushort)'a');
        var vLZ   = Vector128.Create((ushort)'z');

        int i = 0;
        int written = 0;
        int last = src.Length - W;

        while (i <= last)
        {
            var v = Vector128.LoadUnsafe(ref srcRef, (nuint)i);

            var isUpper =
                Vector128.GreaterThanOrEqual(v, vUA)
                & Vector128.LessThanOrEqual(v, vUZ);
            var lowered = v | (isUpper & vCase);

            var isDigit =
                Vector128.GreaterThanOrEqual(lowered, v0)
                & Vector128.LessThanOrEqual(lowered, v9);
            var isLetter =
                Vector128.GreaterThanOrEqual(lowered, vLA)
                & Vector128.LessThanOrEqual(lowered, vLZ);
            var isAlnum = isDigit | isLetter;

            uint mask = isAlnum.ExtractMostSignificantBits();
            if (mask == 0xFF)
            {
                lowered.StoreUnsafe(ref dstRef, (nuint)written);
                written += W;
            }
            else
            {
                // Direct store of compressed vector — see Normalize256
                // for the boundary-safety argument.
                var compressed = Compress128(lowered, mask);
                compressed.StoreUnsafe(ref dstRef, (nuint)written);
                written += BitOperations.PopCount(mask);
            }

            i += W;
        }

        for (; i < src.Length; i++)
        {
            char c = source[i];
            if (AsciiHelpers.IsAlphanumeric(c))
                destination[written++] = AsciiHelpers.ToLowerAsciiInvariant(c);
        }

        return written;
    }

    /// <summary>
    /// Normalize using two interleaved <see cref="Vector128"/> loads per
    /// iteration (16 chars/iter). Aimed at improving instruction-level
    /// parallelism and reducing the per-vector overhead.
    /// </summary>
    public static int Normalize128x2(ReadOnlySpan<char> source, Span<char> destination)
    {
        const int W = 8;
        const int Step = 2 * W;
        var src = MemoryMarshal.Cast<char, ushort>(source);
        var dst = MemoryMarshal.Cast<char, ushort>(destination);

        if (!Vector128.IsHardwareAccelerated || !Ssse3.IsSupported || src.Length < Step)
            return Normalize128(source, destination);

        ref ushort srcRef = ref MemoryMarshal.GetReference(src);
        ref ushort dstRef = ref MemoryMarshal.GetReference(dst);

        var v0    = Vector128.Create((ushort)'0');
        var v9    = Vector128.Create((ushort)'9');
        var vUA   = Vector128.Create((ushort)'A');
        var vUZ   = Vector128.Create((ushort)'Z');
        var vCase = Vector128.Create((ushort)0x20);
        var vLA   = Vector128.Create((ushort)'a');
        var vLZ   = Vector128.Create((ushort)'z');

        int i = 0;
        int written = 0;
        int last = src.Length - Step;

        while (i <= last)
        {
            var v1 = Vector128.LoadUnsafe(ref srcRef, (nuint)i);
            var v2 = Vector128.LoadUnsafe(ref srcRef, (nuint)(i + W));

            var isUpper1 =
                Vector128.GreaterThanOrEqual(v1, vUA)
                & Vector128.LessThanOrEqual(v1, vUZ);
            var isUpper2 =
                Vector128.GreaterThanOrEqual(v2, vUA)
                & Vector128.LessThanOrEqual(v2, vUZ);
            var lowered1 = v1 | (isUpper1 & vCase);
            var lowered2 = v2 | (isUpper2 & vCase);

            var isDigit1 =
                Vector128.GreaterThanOrEqual(lowered1, v0)
                & Vector128.LessThanOrEqual(lowered1, v9);
            var isLetter1 =
                Vector128.GreaterThanOrEqual(lowered1, vLA)
                & Vector128.LessThanOrEqual(lowered1, vLZ);
            var alnum1 = isDigit1 | isLetter1;

            var isDigit2 =
                Vector128.GreaterThanOrEqual(lowered2, v0)
                & Vector128.LessThanOrEqual(lowered2, v9);
            var isLetter2 =
                Vector128.GreaterThanOrEqual(lowered2, vLA)
                & Vector128.LessThanOrEqual(lowered2, vLZ);
            var alnum2 = isDigit2 | isLetter2;

            uint m1 = alnum1.ExtractMostSignificantBits();
            uint m2 = alnum2.ExtractMostSignificantBits();

            if (m1 == 0xFF && m2 == 0xFF)
            {
                lowered1.StoreUnsafe(ref dstRef, (nuint)written);
                lowered2.StoreUnsafe(ref dstRef, (nuint)(written + W));
                written += Step;
            }
            else
            {
                // Direct-store compress for both halves. Safety: written ≤ i
                // ≤ src.Length - Step on entry, so written + W ≤ src.Length
                // - W; after the first store and advance (≤ W), written ≤
                // src.Length - W, so the second store of W ushorts is also
                // in-bounds.
                var c1 = Compress128(lowered1, m1);
                c1.StoreUnsafe(ref dstRef, (nuint)written);
                written += BitOperations.PopCount(m1);
                var c2 = Compress128(lowered2, m2);
                c2.StoreUnsafe(ref dstRef, (nuint)written);
                written += BitOperations.PopCount(m2);
            }

            i += Step;
        }

        if (i < src.Length)
        {
            written += Normalize128(source.Slice(i), destination.Slice(written));
        }

        return written;
    }
}


