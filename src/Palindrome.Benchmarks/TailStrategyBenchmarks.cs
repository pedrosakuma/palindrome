using BenchmarkDotNet.Attributes;
using Palindrome.Core.Strategies;

namespace Palindrome.Benchmarks;

/// <summary>
/// Compares SIMD tail-handling strategies (scalar fallback vs overlap load
/// vs masked-compare) across input sizes that produce a non-empty tail for
/// 128-bit and 256-bit vectors.
/// </summary>
[MemoryDiagnoser]
public class TailStrategyBenchmarks
{
    /// <summary>
    /// Sizes chosen so the tail dominates relative to the body. Includes
    /// off-by-one cases for the V128 (W=8) and V256 (W=16) widths.
    /// </summary>
    [Params(9, 17, 23, 33, 49, 65, 129, 1_024)]
    public int Length { get; set; }

    private string _palindrome = string.Empty;

    private readonly Vector128Checker _v128Scalar = new();
    private readonly Vector128OverlapChecker _v128Overlap = new();
    private readonly Vector256Checker _v256Scalar = new();
    private readonly Vector256OverlapChecker _v256Overlap = new();
    private readonly Vector256MaskedTailChecker _v256Masked = new();

    [GlobalSetup]
    public void Setup() => _palindrome = PalindromeData.Build(Length, seed: 99);

    [Benchmark(Baseline = true)]
    public bool V128_ScalarTail() => _v128Scalar.IsPalindrome(_palindrome);

    [Benchmark]
    public bool V128_OverlapTail() => _v128Overlap.IsPalindrome(_palindrome);

    [Benchmark]
    public bool V256_ScalarTail() => _v256Scalar.IsPalindrome(_palindrome);

    [Benchmark]
    public bool V256_OverlapTail() => _v256Overlap.IsPalindrome(_palindrome);

    [Benchmark]
    public bool V256_MaskedTail() => _v256Masked.IsPalindrome(_palindrome);
}
