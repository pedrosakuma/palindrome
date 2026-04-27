using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Size-aware dispatcher that picks the cheapest strategy for the
/// length being processed. Thresholds were derived from
/// <c>TailStrategyBenchmarks</c> and <c>NormalizationBenchmarks</c>:
///
///   raw length &lt; <see cref="SmallInputThreshold"/>:
///       TwoPointer scalar — avoids the ArrayPool rent + normalize pass.
///
///   normalized length &lt; <see cref="V128MinLength"/>:
///       <see cref="SimdCompare.ScalarCompare"/> — 16 chars don't even
///       fill one V128 register, scalar wins by code-size alone.
///
///   normalized length in [<see cref="V128MinLength"/>, <see cref="V256MinLength"/>):
///       <see cref="SimdCompare.V128_OverlapTail"/> — fastest for 17–512
///       chars in measurements (no scalar tail, single residual vector).
///
///   normalized length ≥ <see cref="V256MinLength"/> on AVX2 hardware:
///       <see cref="SimdCompare.V256_ScalarTail"/> — 16 chars/iter amortizes
///       the small scalar cleanup beyond ~1 KiB.
///
/// Falls back gracefully when V128/V256 are not hardware-accelerated.
/// </summary>
[SkipLocalsInit]
public sealed class HybridChecker : IPalindromeChecker
{
    /// <summary>Inputs shorter than this skip normalization entirely.</summary>
    public const int SmallInputThreshold = 32;

    /// <summary>Minimum normalized length to engage the V128 kernel.</summary>
    public const int V128MinLength = 16;

    /// <summary>Minimum normalized length to engage the V256 kernel.</summary>
    public const int V256MinLength = 512;

    public string Name => "Hybrid (size-aware dispatch)";

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        // Tiny inputs: skip the rent/normalize round-trip.
        if (text.Length < SmallInputThreshold)
            return TwoPointerScalar(text);

        char[] rented = ArrayPool<char>.Shared.Rent(text.Length);
        try
        {
            int len = AsciiNormalizer.Normalize(text, rented);
            var normalized = rented.AsSpan(0, len);
            return Dispatch(normalized);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(rented);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Dispatch(ReadOnlySpan<char> normalized)
    {
        int n = normalized.Length;

        if (n < V128MinLength || !Vector128.IsHardwareAccelerated)
            return SimdCompare.ScalarCompare(normalized);

        if (n < V256MinLength || !Vector256.IsHardwareAccelerated)
            return SimdCompare.V128_OverlapTail(normalized);

        return SimdCompare.V256_ScalarTail(normalized);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TwoPointerScalar(ReadOnlySpan<char> text)
    {
        int left = 0;
        int right = text.Length - 1;

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
}
