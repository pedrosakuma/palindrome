using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Palindrome.Core;
using Palindrome.Core.Strategies;

namespace Palindrome.Benchmarks;

[Config(typeof(Config))]
[MemoryDiagnoser]
public class PalindromeBenchmarks
{
    private sealed class Config : ManualConfig
    {
        public Config()
        {
            AddJob(Job.Default.WithId("net10"));
        }
    }

    /// <summary>
    /// Sentence sizes to benchmark. Includes a tiny phrase (cache-friendly,
    /// stresses overhead) and progressively larger inputs to expose SIMD gains.
    /// </summary>
    [Params(64, 1_024, 16_384, 262_144)]
    public int Length { get; set; }

    private string _palindrome = string.Empty;

    private readonly NaiveAllocatingChecker _naive = new();
    private readonly TwoPointerChecker _twoPointer = new();
    private readonly NormalizedTwoPointerChecker _normalizedTwoPointer = new();
    private readonly VectorTChecker _vectorT = new();
    private readonly Vector128Checker _v128 = new();
    private readonly Vector256Checker _v256 = new();
    private readonly Vector512Checker _v512 = new();
    // HybridChecker is intentionally NOT benchmarked here:
    // it dispatches between scalar/V128/V256 *after* a full normalization
    // pass and was strictly dominated by TwoPointerSimdChecker (which fuses
    // normalization with the comparison). Keeping the file in Core for
    // documentation of the size-dispatch experiment, but excluding it from
    // the suite avoids spending ~3 minutes per run on a known-loser.
    private readonly TwoPointerSimdChecker _twoPointerSimd = new();
    private readonly TwoPointerSimdByteChecker _twoPointerSimdByte = new();
    private readonly TwoPointerSimdAvx512Checker _twoPointerSimdAvx512 = new();

    [GlobalSetup]
    public void Setup() => _palindrome = PalindromeData.Build(Length, seed: 42);

    [Benchmark(Baseline = true)]
    public bool Naive() => _naive.IsPalindrome(_palindrome);

    [Benchmark]
    public bool TwoPointer() => _twoPointer.IsPalindrome(_palindrome);

    [Benchmark]
    public bool NormalizedTwoPointer() => _normalizedTwoPointer.IsPalindrome(_palindrome);

    [Benchmark]
    public bool VectorT() => _vectorT.IsPalindrome(_palindrome);

    [Benchmark]
    public bool Vector128() => _v128.IsPalindrome(_palindrome);

    [Benchmark]
    public bool Vector256() => _v256.IsPalindrome(_palindrome);

    [Benchmark]
    public bool Vector512() => _v512.IsPalindrome(_palindrome);

    [Benchmark]
    public bool TwoPointerSimd() => _twoPointerSimd.IsPalindrome(_palindrome);

    [Benchmark]
    public bool TwoPointerSimdByte() => _twoPointerSimdByte.IsPalindrome(_palindrome);

    [Benchmark]
    public bool TwoPointerSimdAvx512() => _twoPointerSimdAvx512.IsPalindrome(_palindrome);
}
