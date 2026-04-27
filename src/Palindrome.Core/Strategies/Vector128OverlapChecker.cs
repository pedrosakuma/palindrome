using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Vector128 palindrome compare that handles the unaligned middle by issuing
/// one final vector load that overlaps the previous iteration. Removes the
/// scalar tail entirely whenever the input has at least one full vector.
/// </summary>
public sealed class Vector128OverlapChecker : IPalindromeChecker
{
    public string Name => "Vector128<ushort> + overlap tail";

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

        if (!Vector128.IsHardwareAccelerated || data.Length < W)
            return ScalarFallback(data);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);

        int left = 0;
        int right = data.Length - W;

        // Strictly non-overlapping iterations.
        while (left + W < right)
        {
            var lv = Vector128.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector128.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Vector128.Shuffle(rv, ReverseIndices))
                return false;
            left += W;
            right -= W;
        }

        // One final compare that may re-check already-validated positions.
        // Safe because palindrome equality is idempotent.
        int finalLeft = (data.Length - W) / 2;
        int finalRight = data.Length - finalLeft - W;
        var fl = Vector128.LoadUnsafe(ref baseRef, (nuint)finalLeft);
        var fr = Vector128.LoadUnsafe(ref baseRef, (nuint)finalRight);
        return fl == Vector128.Shuffle(fr, ReverseIndices);
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
