using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Palindrome.Core;

namespace Palindrome.Benchmarks;

/// <summary>
/// Tiny, single-shape benchmark used purely to dump JIT assembly for the
/// SIMD kernels and the SIMD normalizer. Runs a short job to keep the
/// turnaround low.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RunStrategy.Throughput, warmupCount: 3, iterationCount: 5, invocationCount: 16384)]
[DisassemblyDiagnoser(maxDepth: 3, exportGithubMarkdown: true, printSource: true)]
public class AsmDumpBenchmarks
{
    private const int Length = 1024;

    private string _normalized = string.Empty;
    private string _withSymbols = string.Empty;
    private char[] _dest = [];

    [GlobalSetup]
    public void Setup()
    {
        _normalized = PalindromeData.Build(Length, seed: 1);
        _withSymbols = BuildWithSymbols(Length, density: 0.05, seed: 2);
        _dest = new char[Length];
    }

    private static string BuildWithSymbols(int length, double density, int seed)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const string symbols = " ,.!?;:'";
        var rng = new Random(seed);
        var sb = new System.Text.StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            sb.Append(rng.NextDouble() < density
                ? symbols[rng.Next(symbols.Length)]
                : letters[rng.Next(letters.Length)]);
        }
        return sb.ToString();
    }

    // ---- Comparison kernels (already-normalized input) ----

    [Benchmark]
    public bool Cmp_Scalar() => SimdCompare.ScalarCompare(_normalized);

    [Benchmark]
    public bool Cmp_V128_Overlap() => SimdCompare.V128_OverlapTail(_normalized);

    [Benchmark]
    public bool Cmp_V256_Overlap() => SimdCompare.V256_OverlapTail(_normalized);

    [Benchmark]
    public bool Cmp_V256_Masked() => SimdCompare.V256_MaskedTail(_normalized);

    // ---- Normalization kernels (raw input with symbols) ----

    [Benchmark]
    public int Norm_Scalar() => AsciiNormalizer.Normalize(_withSymbols, _dest);

    [Benchmark]
    public int Norm_Simd128() => AsciiNormalizerSimd.Normalize128(_withSymbols, _dest);

    [Benchmark]
    public int Norm_Simd128x2() => AsciiNormalizerSimd.Normalize128x2(_withSymbols, _dest);

    [Benchmark]
    public int Norm_Simd256() => AsciiNormalizerSimd.Normalize256(_withSymbols, _dest);
}
