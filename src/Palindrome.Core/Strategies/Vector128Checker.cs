using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Compares the normalized buffer using fixed-width 128-bit vectors of
/// <see cref="ushort"/> (8 chars per iteration). Falls back to scalar
/// when the platform lacks 128-bit acceleration or the input is too small.
/// </summary>
public sealed class Vector128Checker : IPalindromeChecker
{
    public string Name => "Vector128<ushort> + scalar tail";

    private static readonly Vector128<ushort> ReverseIndices =
        Vector128.Create((ushort)7, 6, 5, 4, 3, 2, 1, 0);

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
        const int W = 8;
        var data = MemoryMarshal.Cast<char, ushort>(span);

        if (!Vector128.IsHardwareAccelerated || data.Length < W * 2)
            return ScalarCompare(data);

        int left = 0;
        int right = data.Length - W;

        while (left < right)
        {
            var lv = Vector128.LoadUnsafe(ref MemoryMarshal.GetReference(data), (nuint)left);
            var rv = Vector128.LoadUnsafe(ref MemoryMarshal.GetReference(data), (nuint)right);
            var reversed = Vector128.Shuffle(rv, ReverseIndices);

            if (lv != reversed)
                return false;

            left += W;
            right -= W;
        }

        // Handle the middle slice that may not cover a full vector.
        if (left < right + W)
            return ScalarCompare(data.Slice(left, (right + W) - left));

        return true;
    }

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
