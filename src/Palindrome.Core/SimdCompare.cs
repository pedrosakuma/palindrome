using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Palindrome.Core;

/// <summary>
/// Direct entry points for the SIMD comparison kernels operating on a
/// pre-normalized character buffer. Used by benchmarks to isolate the
/// comparison cost from the normalization cost.
/// </summary>
[SkipLocalsInit]
public static class SimdCompare
{
    // Byte-level reverse pattern for 8 ushorts inside a 128-bit lane:
    // ushort i (bytes [2i, 2i+1]) goes to position 7-i.
    private static readonly Vector128<byte> ReverseBytes128 = Vector128.Create(
        (byte)14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

    // For 16 ushorts in 256 bits: first swap the two 128-bit halves with
    // vperm2i128, then apply the per-lane reverse above to both halves.
    private static readonly Vector256<byte> ReverseBytes256 = Vector256.Create(
        (byte)14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1,
                14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

    // Fallback shuffle indices for portable Vector*.Shuffle path.
    private static readonly Vector128<ushort> ReverseIndices128 =
        Vector128.Create((ushort)7, 6, 5, 4, 3, 2, 1, 0);

    private static readonly Vector256<ushort> ReverseIndices256 = Vector256.Create(
        (ushort)15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0);

    private const int W256 = 16;
    private static readonly Vector256<ushort>[] TailMasks256 = BuildMasks256();

    private static Vector256<ushort>[] BuildMasks256()
    {
        var masks = new Vector256<ushort>[W256 + 1];
        Span<ushort> buf = stackalloc ushort[W256];
        for (int n = 0; n <= W256; n++)
        {
            buf.Clear();
            for (int i = 0; i < n; i++) buf[i] = 0xFFFF;
            masks[n] = Vector256.Create<ushort>(buf);
        }
        return masks;
    }

    // ---------------- Reverse helpers ----------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ushort> Reverse(Vector128<ushort> v)
    {
        if (Ssse3.IsSupported)
            return Ssse3.Shuffle(v.AsByte(), ReverseBytes128).AsUInt16();
        return Vector128.Shuffle(v, ReverseIndices128);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector256<ushort> Reverse(Vector256<ushort> v)
    {
        if (Avx2.IsSupported)
        {
            // Swap the two 128-bit halves then reverse 8 ushorts in each.
            var swapped = Avx2.Permute2x128(v.AsByte(), v.AsByte(), 0x01);
            return Avx2.Shuffle(swapped, ReverseBytes256).AsUInt16();
        }
        return Vector256.Shuffle(v, ReverseIndices256);
    }

    // ---------------- Scalar baseline ----------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ScalarCompare(ReadOnlySpan<char> normalized)
    {
        var data = MemoryMarshal.Cast<char, ushort>(normalized);
        int l = 0, r = data.Length - 1;
        while (l < r)
        {
            if (data[l] != data[r]) return false;
            l++; r--;
        }
        return true;
    }

    // ---------------- Vector128 ----------------

    public static bool V128_ScalarTail(ReadOnlySpan<char> normalized)
    {
        const int W = 8;
        var data = MemoryMarshal.Cast<char, ushort>(normalized);
        if (data.Length < W * 2) return ScalarCompare(normalized);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);
        int left = 0, right = data.Length - W;
        while (left < right)
        {
            var lv = Vector128.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector128.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Reverse(rv)) return false;
            left += W; right -= W;
        }
        if (left < right + W)
        {
            var slice = data.Slice(left, (right + W) - left);
            int l = 0, r = slice.Length - 1;
            while (l < r) { if (slice[l] != slice[r]) return false; l++; r--; }
        }
        return true;
    }

    public static bool V128_OverlapTail(ReadOnlySpan<char> normalized)
    {
        const int W = 8;
        var data = MemoryMarshal.Cast<char, ushort>(normalized);
        if (data.Length < W) return ScalarCompare(normalized);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);
        int left = 0, right = data.Length - W;
        while (left + W < right)
        {
            var lv = Vector128.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector128.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Reverse(rv)) return false;
            left += W; right -= W;
        }
        int finalLeft = (data.Length - W) / 2;
        int finalRight = data.Length - finalLeft - W;
        var fl = Vector128.LoadUnsafe(ref baseRef, (nuint)finalLeft);
        var fr = Vector128.LoadUnsafe(ref baseRef, (nuint)finalRight);
        return fl == Reverse(fr);
    }

    // ---------------- Vector256 ----------------

    public static bool V256_ScalarTail(ReadOnlySpan<char> normalized)
    {
        const int W = 16;
        var data = MemoryMarshal.Cast<char, ushort>(normalized);
        if (data.Length < W * 2) return ScalarCompare(normalized);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);
        int left = 0, right = data.Length - W;
        while (left < right)
        {
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Reverse(rv)) return false;
            left += W; right -= W;
        }
        if (left < right + W)
        {
            var slice = data.Slice(left, (right + W) - left);
            int l = 0, r = slice.Length - 1;
            while (l < r) { if (slice[l] != slice[r]) return false; l++; r--; }
        }
        return true;
    }

    public static bool V256_OverlapTail(ReadOnlySpan<char> normalized)
    {
        const int W = 16;
        var data = MemoryMarshal.Cast<char, ushort>(normalized);
        if (data.Length < W) return ScalarCompare(normalized);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);
        int left = 0, right = data.Length - W;
        while (left + W < right)
        {
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Reverse(rv)) return false;
            left += W; right -= W;
        }
        int finalLeft = (data.Length - W) / 2;
        int finalRight = data.Length - finalLeft - W;
        var fl = Vector256.LoadUnsafe(ref baseRef, (nuint)finalLeft);
        var fr = Vector256.LoadUnsafe(ref baseRef, (nuint)finalRight);
        return fl == Reverse(fr);
    }

    public static bool V256_MaskedTail(ReadOnlySpan<char> normalized)
    {
        const int W = 16;
        var data = MemoryMarshal.Cast<char, ushort>(normalized);
        if (data.Length < W * 2) return ScalarCompare(normalized);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);
        int left = 0, right = data.Length - W;
        while (left + W <= right)
        {
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Reverse(rv)) return false;
            left += W; right -= W;
        }
        int tail = data.Length - 2 * left;
        if (tail >= 2)
        {
            int leftPos = left;
            int rightPos = data.Length - left - W;
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)leftPos);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)rightPos);
            var diff = lv ^ Reverse(rv);
            int validLanes = (tail + 1) / 2;
            if ((diff & TailMasks256[validLanes]) != Vector256<ushort>.Zero) return false;
        }
        return true;
    }
}
