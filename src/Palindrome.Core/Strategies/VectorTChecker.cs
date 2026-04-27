using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Normalizes the input then compares both ends using <see cref="Vector{T}"/>
/// of <see cref="ushort"/>. The reverse of the right-hand window is performed
/// with a real SIMD shuffle (dispatched to the matching fixed-width vector
/// type), not a stack copy.
/// </summary>
public sealed class VectorTChecker : IPalindromeChecker
{
    public string Name => $"Vector<ushort> (width={Vector<ushort>.Count})";

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        char[] rented = ArrayPool<char>.Shared.Rent(text.Length);
        try
        {
            int len = AsciiNormalizer.Normalize(text, rented);
            return CompareReversed(rented.AsSpan(0, len));
        }
        finally
        {
            ArrayPool<char>.Shared.Return(rented);
        }
    }

    private static bool CompareReversed(ReadOnlySpan<char> span)
    {
        int width = Vector<ushort>.Count;
        var data = MemoryMarshal.Cast<char, ushort>(span);

        if (!Vector.IsHardwareAccelerated || data.Length < width * 2)
            return ScalarCompare(data);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);
        int left = 0;
        int right = data.Length - width;

        // Dispatch to the matching fixed-width SIMD shuffle exactly once.
        if (width == 16 && Vector256.IsHardwareAccelerated)
        {
            while (left < right)
            {
                var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
                var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)right);
                if (lv != Vector256.Shuffle(rv, ReverseIndices256))
                    return false;
                left += width; right -= width;
            }
        }
        else if (width == 8 && Vector128.IsHardwareAccelerated)
        {
            while (left < right)
            {
                var lv = Vector128.LoadUnsafe(ref baseRef, (nuint)left);
                var rv = Vector128.LoadUnsafe(ref baseRef, (nuint)right);
                if (lv != Vector128.Shuffle(rv, ReverseIndices128))
                    return false;
                left += width; right -= width;
            }
        }
        else if (width == 32 && Vector512.IsHardwareAccelerated)
        {
            while (left < right)
            {
                var lv = Vector512.LoadUnsafe(ref baseRef, (nuint)left);
                var rv = Vector512.LoadUnsafe(ref baseRef, (nuint)right);
                if (lv != Vector512.Shuffle(rv, ReverseIndices512))
                    return false;
                left += width; right -= width;
            }
        }
        else
        {
            // Exotic widths (Wasm, NEON variations): fall back to scalar.
            return ScalarCompare(data);
        }

        if (left < right + width)
            return ScalarCompare(data.Slice(left, (right + width) - left));

        return true;
    }

    private static readonly Vector128<ushort> ReverseIndices128 =
        Vector128.Create((ushort)7, 6, 5, 4, 3, 2, 1, 0);

    private static readonly Vector256<ushort> ReverseIndices256 = Vector256.Create(
        (ushort)15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0);

    private static readonly Vector512<ushort> ReverseIndices512 = Vector512.Create(
        (ushort)31, 30, 29, 28, 27, 26, 25, 24,
        23, 22, 21, 20, 19, 18, 17, 16,
        15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ScalarCompare(ReadOnlySpan<ushort> data)
    {
        int l = 0, r = data.Length - 1;
        while (l < r)
        {
            if (data[l] != data[r]) return false;
            l++; r--;
        }
        return true;
    }
}
