using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Vector256 compare with a precomputed mask table for the unaligned tail.
/// Loads the full vector at the boundary and ANDs the XOR-difference with a
/// mask that zeroes lanes outside the still-unchecked middle, then tests for
/// any non-zero lane. Useful when re-reading is undesirable; on x86 this is
/// software-emulated since AVX2 has no native ushort masked compare.
/// </summary>
public sealed class Vector256MaskedTailChecker : IPalindromeChecker
{
    public string Name => "Vector256<ushort> + masked tail";

    public static bool IsSupported => Vector256.IsHardwareAccelerated;

    private const int W = 16;

    private static readonly Vector256<ushort> ReverseIndices = Vector256.Create(
        (ushort)15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0);

    // TailMasks[n] has the first n lanes set to 0xFFFF, the rest 0.
    private static readonly Vector256<ushort>[] TailMasks = BuildMasks();

    private static Vector256<ushort>[] BuildMasks()
    {
        var masks = new Vector256<ushort>[W + 1];
        Span<ushort> buf = stackalloc ushort[W];
        for (int n = 0; n <= W; n++)
        {
            buf.Clear();
            for (int i = 0; i < n; i++) buf[i] = 0xFFFF;
            masks[n] = Vector256.Create<ushort>(buf);
        }
        return masks;
    }

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
        var data = MemoryMarshal.Cast<char, ushort>(span);

        if (!Vector256.IsHardwareAccelerated || data.Length < 2 * W)
            return ScalarFallback(data);

        ref ushort baseRef = ref MemoryMarshal.GetReference(data);

        int left = 0;
        int right = data.Length - W;

        while (left + W <= right)
        {
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)left);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)right);
            if (lv != Vector256.Shuffle(rv, ReverseIndices))
                return false;
            left += W;
            right -= W;
        }

        // Unchecked middle = positions [left, data.Length - left).
        int tail = data.Length - 2 * left;
        if (tail >= 2)
        {
            // Half of `tail` lanes carry meaningful comparisons; the symmetric
            // pair is loaded from the mirrored window.
            int leftPos = left;
            int rightPos = data.Length - left - W;
            var lv = Vector256.LoadUnsafe(ref baseRef, (nuint)leftPos);
            var rv = Vector256.LoadUnsafe(ref baseRef, (nuint)rightPos);
            var diff = lv ^ Vector256.Shuffle(rv, ReverseIndices);
            // Only the first ceil(tail/2) lanes correspond to unchecked pairs;
            // mask out the rest. Half-pairs round up to be safe.
            int validLanes = (tail + 1) / 2;
            var masked = diff & TailMasks[validLanes];
            if (masked != Vector256<ushort>.Zero) return false;
        }

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
