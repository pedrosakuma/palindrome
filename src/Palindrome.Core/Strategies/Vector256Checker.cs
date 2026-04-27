using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Compares the normalized buffer using 256-bit vectors of <see cref="ushort"/>
/// (16 chars per iteration). Requires 256-bit hardware acceleration.
/// </summary>
public sealed class Vector256Checker : IPalindromeChecker
{
    public string Name => "Vector256<ushort> + scalar tail";

    public static bool IsSupported => Vector256.IsHardwareAccelerated;

    private static readonly Vector256<ushort> ReverseIndices = Vector256.Create(
        (ushort)15, 14, 13, 12, 11, 10, 9, 8,
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
        const int W = 16;
        var data = MemoryMarshal.Cast<char, ushort>(span);

        if (!Vector256.IsHardwareAccelerated || data.Length < W * 2)
            return ScalarFallback(data);

        int left = 0;
        int right = data.Length - W;

        while (left < right)
        {
            var lv = Vector256.LoadUnsafe(ref MemoryMarshal.GetReference(data), (nuint)left);
            var rv = Vector256.LoadUnsafe(ref MemoryMarshal.GetReference(data), (nuint)right);
            var reversed = Vector256.Shuffle(rv, ReverseIndices);

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
