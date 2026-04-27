using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Compares the normalized buffer using 512-bit vectors of <see cref="ushort"/>
/// (32 chars per iteration). Requires AVX-512 (or equivalent) acceleration.
/// </summary>
public sealed class Vector512Checker : IPalindromeChecker
{
    public string Name => "Vector512<ushort> + scalar tail";

    public static bool IsSupported => Vector512.IsHardwareAccelerated;

    private static readonly Vector512<ushort> ReverseIndices = Vector512.Create(
        (ushort)31, 30, 29, 28, 27, 26, 25, 24,
        23, 22, 21, 20, 19, 18, 17, 16,
        15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0);

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        char[] rented = ArrayPool<char>.Shared.Rent(text.Length);
        try
        {
            int len = AsciiNormalizer.Normalize(text, rented);
            return Compare(rented.AsSpan(0, len));
        }
        finally
        {
            ArrayPool<char>.Shared.Return(rented);
        }
    }

    private static bool Compare(ReadOnlySpan<char> span)
    {
        const int W = 32;
        var data = MemoryMarshal.Cast<char, ushort>(span);

        if (!Vector512.IsHardwareAccelerated || data.Length < W * 2)
            return ScalarFallback(data);

        int left = 0;
        int right = data.Length - W;

        while (left < right)
        {
            var lv = Vector512.LoadUnsafe(ref MemoryMarshal.GetReference(data), (nuint)left);
            var rv = Vector512.LoadUnsafe(ref MemoryMarshal.GetReference(data), (nuint)right);
            var reversed = Vector512.Shuffle(rv, ReverseIndices);

            if (lv != reversed)
                return false;

            left += W;
            right -= W;
        }

        if (left < right + W)
            return ScalarFallback(data.Slice(left, (right + W) - left));

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ScalarFallback(ReadOnlySpan<ushort> data)
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
