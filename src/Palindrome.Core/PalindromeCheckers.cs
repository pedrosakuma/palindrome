using Palindrome.Core.Strategies;

namespace Palindrome.Core;

/// <summary>
/// Convenience entry point that exposes every available palindrome strategy.
/// </summary>
public static class PalindromeCheckers
{
    public static IReadOnlyList<IPalindromeChecker> All { get; } = new IPalindromeChecker[]
    {
        new NaiveAllocatingChecker(),
        new TwoPointerChecker(),
        new NormalizedTwoPointerChecker(),
        new VectorTChecker(),
        new Vector128Checker(),
        new Vector128OverlapChecker(),
        new Vector256Checker(),
        new Vector256OverlapChecker(),
        new Vector256MaskedTailChecker(),
        new Vector512Checker(),
        new HybridChecker(),
        new TwoPointerSimdChecker(),
        new TwoPointerSimdByteChecker(),
        new TwoPointerSimdAvx512Checker(),
    };
}
