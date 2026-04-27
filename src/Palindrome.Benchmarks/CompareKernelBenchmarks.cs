using BenchmarkDotNet.Attributes;
using Palindrome.Core;

namespace Palindrome.Benchmarks;

/// <summary>
/// Benchmarks the comparison kernels in isolation, on a buffer that is
/// already normalized. Removes the normalization cost so the SIMD/tail
/// differences are visible.
/// </summary>
[MemoryDiagnoser]
public class CompareKernelBenchmarks
{
    [Params(8, 9, 16, 17, 23, 32, 33, 49, 64, 65, 128, 129, 1_024)]
    public int Length { get; set; }

    private string _normalized = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        // Build a palindrome WITHOUT symbols so it's already "normalized".
        var rng = new Random(7);
        const string alpha = "abcdefghijklmnopqrstuvwxyz0123456789";
        int half = Length / 2;
        var sb = new System.Text.StringBuilder(Length);
        var first = new char[half];
        for (int i = 0; i < half; i++) first[i] = alpha[rng.Next(alpha.Length)];
        sb.Append(first);
        if (Length % 2 == 1) sb.Append('x');
        for (int i = half - 1; i >= 0; i--) sb.Append(first[i]);
        _normalized = sb.ToString();
    }

    [Benchmark(Baseline = true)]
    public bool Scalar() => SimdCompare.ScalarCompare(_normalized);

    [Benchmark]
    public bool V128_ScalarTail() => SimdCompare.V128_ScalarTail(_normalized);

    [Benchmark]
    public bool V128_OverlapTail() => SimdCompare.V128_OverlapTail(_normalized);

    [Benchmark]
    public bool V256_ScalarTail() => SimdCompare.V256_ScalarTail(_normalized);

    [Benchmark]
    public bool V256_OverlapTail() => SimdCompare.V256_OverlapTail(_normalized);

    [Benchmark]
    public bool V256_MaskedTail() => SimdCompare.V256_MaskedTail(_normalized);
}
