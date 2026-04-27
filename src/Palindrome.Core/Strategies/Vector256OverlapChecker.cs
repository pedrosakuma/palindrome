using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Vector256 palindrome compare with overlap-tail strategy: the trailing
/// chunk is handled by a single straddling vector load that may re-check
/// already-validated positions, eliminating the scalar tail.
/// </summary>
public sealed class Vector256OverlapChecker : IPalindromeChecker
{
    public string Name => "Vector256<ushort> + overlap tail";

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

        if (!Vector256.IsHardwareAccelerated || data.Length < W)
            return ScalarFallback(data);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);

        int left = 0;
        int right = data.Length - W;

        while (left + W < right)
        {
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Vector256.Shuffle(rv, ReverseIndices))
                return false;
            left += W;
            right -= W;
        }

        int finalLeft = (data.Length - W) / 2;
        int finalRight = data.Length - finalLeft - W;
        var fl = Vector256.LoadUnsafe(ref baseRef, (nuint)finalLeft);
        var fr = Vector256.LoadUnsafe(ref baseRef, (nuint)finalRight);
        return fl == Vector256.Shuffle(fr, ReverseIndices);
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
