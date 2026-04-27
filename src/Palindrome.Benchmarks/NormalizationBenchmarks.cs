using BenchmarkDotNet.Attributes;
using Palindrome.Core;

namespace Palindrome.Benchmarks;

/// <summary>
/// Isolated normalization benchmarks: scalar vs SIMD bulk-fast-path with
/// varying density of non-alphanumeric symbols. Allocation-free (writes into
/// a preallocated destination buffer).
/// </summary>
[MemoryDiagnoser]
public class NormalizationBenchmarks
{
    [Params(64, 1_024, 16_384)]
    public int Length { get; set; }

    /// <summary>Approximate fraction of symbols (non-alphanumerics) in the input.</summary>
    [Params(0.0, 0.05, 0.30)]
    public double SymbolDensity { get; set; }

    private string _text = string.Empty;
    private char[] _dest = [];

    [GlobalSetup]
    public void Setup()
    {
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const string symbols = " ,.!?;:'\"()[]";
        var rng = new Random(123);
        var sb = new System.Text.StringBuilder(Length);
        for (int i = 0; i < Length; i++)
        {
            sb.Append(rng.NextDouble() < SymbolDensity
                ? symbols[rng.Next(symbols.Length)]
                : letters[rng.Next(letters.Length)]);
        }
        _text = sb.ToString();
        _dest = new char[Length];
    }

    [Benchmark(Baseline = true)]
    public int Scalar() => AsciiNormalizer.Normalize(_text, _dest);

    [Benchmark]
    public int Simd128() => AsciiNormalizerSimd.Normalize128(_text, _dest);

    [Benchmark]
    public int Simd128x2() => AsciiNormalizerSimd.Normalize128x2(_text, _dest);

    [Benchmark]
    public int Simd256() => AsciiNormalizerSimd.Normalize256(_text, _dest);
}
