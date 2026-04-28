# Palindrome — SIMD strategy zoo

[![CI](https://github.com/pedrosakuma/palindrome/actions/workflows/ci.yml/badge.svg)](https://github.com/pedrosakuma/palindrome/actions/workflows/ci.yml)
[![Benchmarks](https://img.shields.io/badge/benchmarks-published-blue)](https://pedrosakuma.github.io/palindrome/dev/bench/)

Educational/demonstration project: progressively faster ways to answer
"is this string a palindrome (case-insensitive, alphanumeric only)?" in
.NET 10, from the textbook scalar two-pointer to AVX-512 byte-narrowed
two-pointer with cross-lane permutes.

> ⚠️ **Numbers in this README are reference points from one machine.**
> CI benchmarks run on GitHub-hosted runners which mix Intel Cascade/Ice
> Lake (AVX-512 ✓) and AMD EPYC 7763 (no AVX-512). Ranking can shift
> across CPU families; treat the published trend chart as indicative.

## Strategies

| # | Strategy | Approach |
|--:|--|--|
| 1 | `NaiveAllocatingChecker` | Allocate a normalized string, reverse it, compare. |
| 2 | `TwoPointerChecker` | Scalar two-pointer with skip on non-alnum. |
| 3 | `NormalizedTwoPointerChecker` | Normalize into stack/pool buffer, scalar two-pointer. |
| 4 | `VectorTChecker` | `Vector<T>` (runtime-sized). |
| 5 | `Vector128Checker` / `Vector128OverlapChecker` | Fixed 128-bit, optionally with end overlap. |
| 6 | `Vector256Checker` / `…OverlapChecker` / `…MaskedTailChecker` | 256-bit variants. |
| 7 | `Vector512Checker` | 512-bit (emulated when AVX-512 absent). |
| 8 | `HybridChecker` | Size-dispatched scalar/V128/V256 (kept for documentation; excluded from headline benchmarks — see code comment). |
| 9 | **`TwoPointerSimdChecker`** | SIMD two-pointer in **ushort** lanes; vectorized symbol skip + popcount-driven advance. |
| 10 | **`TwoPointerSimdByteChecker`** | Same as #9 but **packs ushort→byte** for 2× lane density. |
| 11 | **`TwoPointerSimdAvx512Checker`** | AVX-512 BW + VBMI version of #10: 64 chars/side/iter, single-instruction `vpermb` reverse. Falls back to #10 on unsupported hardware. |
| 12 | `TwoPointerSimdAvx512x2Checker` | Double-pumped AVX-512 (128 chars/side/iter). **Documented negative result** — slower than #11 on Ice Lake-SP because L2 bandwidth, not chain latency, was the real bottleneck. Kept as a cautionary tale; see source. |

## Local benchmarks (AMD EPYC 7763 Zen 3, no AVX-512)

Headline numbers on a 256 KiB clean palindrome (no symbols), median of
3 launches × default BDN job:

| Strategy | Mean | vs Naive |
|--|--:|--:|
| Naive               | 3,513 µs |  1.00× |
| TwoPointer (scalar) |   877 µs |  4.0× |
| Vector128           | 1,075 µs |  3.3× |
| Vector256           | 1,028 µs |  3.4× |
| Vector512 (emulated)| 1,089 µs |  3.2× |
| **TwoPointerSimd**           | **319 µs** | **11.0×** |
| **TwoPointerSimdByte**       | **280 µs** | **12.5×** |
| **TwoPointerSimdAvx512**     | ≈ 280 µs (falls back to byte on Zen 3) |

## Cross-architecture comparison (Zen 3 vs Ice Lake-SP)

Same binary, BenchmarkDotNet `--job short`, 4 input sizes. "Local" is
AMD EPYC 7763 (Zen 3, no AVX-512); "Azure" is Intel Xeon Platinum 8370C
(Ice Lake-SP Standard_D2s_v5, AVX-512BW + VBMI present).

| Length | Strategy                | Local (Zen 3) | Azure (Ice Lake-SP) | Δ Azure |
|-------:|-------------------------|--------------:|--------------------:|--------:|
|     64 | TwoPointerSimd          |       15.3 ns |             11.5 ns |   −25%  |
|     64 | **TwoPointerSimdByte**  |    **8.5 ns** |          **5.7 ns** |   −33%  |
|     64 | TwoPointerSimdAvx512    |   ↳ byte path |             63.1 ns |   slow  |
|   1024 | TwoPointerSimd          |        994 ns |          **861 ns** |   −13%  |
|   1024 | TwoPointerSimdByte      |       1083 ns |              973 ns |   −10%  |
|   1024 | TwoPointerSimdAvx512    |   ↳ byte path |             1002 ns |   slow  |
|  16384 | **TwoPointerSimd**      |   **14.6 µs** |             14.1 µs |   ≈     |
|  16384 | TwoPointerSimdByte      |       15.6 µs |             15.1 µs |   ≈     |
|  16384 | TwoPointerSimdAvx512    |   ↳ byte path |             15.8 µs |   ≈     |
| 262144 | TwoPointerSimd          |        267 µs |          **235 µs** |   −12%  |
| 262144 | **TwoPointerSimdByte**  |    **261 µs** |              243 µs |    −7%  |
| 262144 | TwoPointerSimdAvx512    |   ↳ byte path |              256 µs |   slow  |
|     64 | Naive (sanity baseline) |        776 ns |              763 ns |   ≈     |

**Three things worth noting:**

1. **Best strategy depends on the silicon.** On Zen 3, byte-narrowing
   wins at every size — Zen 3 has wide V256 throughput and short
   `vpackuswb` latency, so doubling lane density actually doubles work
   per cycle. On Ice Lake-SP, the V128 path wins at every size ≥ 1 KiB
   because Ice Lake-SP throttles its zmm/ymm clock and the V128 chain
   stays at boost.
2. **AVX-512 explicit intrinsics did not pay off on Ice Lake-SP for this
   workload.** Both `TwoPointerSimdAvx512` and the experimental
   `TwoPointerSimdAvx512x2` (kept as a documented negative result, see
   its source) are slower than the V128 baseline. The bottleneck is
   memory bandwidth past L1, not vector chain latency, and the
   AVX-512 frequency penalty wipes out the wider lanes.
3. **Naive identical across both** (776 vs 763 ns @ 64) confirms the
   scalar/clock baseline is essentially the same — the deltas above
   are all SIMD-execution differences, not "Azure is faster".

Conclusion: ship `TwoPointerSimdChecker` (V128) as the portable default,
keep `TwoPointerSimdByteChecker` for Zen 3 / wide-V256 hosts, and treat
the AVX-512 paths as documented experiments rather than production
defaults.

See the published [bench trend](https://pedrosakuma.github.io/palindrome/dev/bench/) for
the per-commit Azure series.

## Key techniques along the way

- **Variable-advance two-pointer with `BitOperations.TrailingZeroCount`** —
  beats all-or-nothing fast paths on dirty input by computing the length
  of the longest run of alnum lanes from the start of each side.
- **Case-fold via `v | 0x20`** plus a single range check on the lowercase
  alphabet covers both `A-Z` and `a-z` in one compare.
- **`Ssse3.Shuffle` (pshufb) compress table** in `AsciiNormalizerSimd`:
  256-entry × 16-byte LUT (4 KiB, fits in L1) emulating AVX-512's
  `vpcompressw` to gather active lanes in the slow path.
- **Byte narrowing via `vpackuswb` + `vpermq`** doubles effective lane
  count when input is known-ASCII (high byte == 0).
- **`vpermb` (AVX-512VBMI)** performs a 64-byte cross-lane reverse in a
  single instruction — vs the AVX2 sequence of `vpermq` + `vpshufb`.
- **`SkipLocalsInit`** to avoid the cost of zero-init on stackalloc'd
  scratch buffers in the hot path.

## Repository layout

```
src/
  Palindrome.Core/         # Strategies + helpers
    Strategies/            # One file per IPalindromeChecker
    AsciiNormalizerSimd.cs # SIMD compress-based normalizer
    AsciiNormalizer.cs     # Scalar baseline normalizer
  Palindrome.Benchmarks/   # BenchmarkDotNet harness
tests/
  Palindrome.Tests/        # xUnit, parameterised over every checker
.github/workflows/ci.yml   # Test on every PR; bench on main + dispatch
```

## Running locally

```bash
# Tests
dotnet test tests/Palindrome.Tests -c Release

# Headline benchmark (palindrome detection across 4 input sizes)
dotnet run -c Release --project src/Palindrome.Benchmarks -- \
  --filter '*PalindromeBenchmarks*' --job short

# Just the normalizer micro-benchmarks
dotnet run -c Release --project src/Palindrome.Benchmarks -- \
  --filter '*NormalizationBenchmarks*' --job short
```

`DOTNET_TieredCompilation=0` is recommended when comparing strategies —
tier-0 codegen for SIMD is misleadingly slow.

## CI

The workflow at `.github/workflows/ci.yml`:

- **Tests** on every PR and push.
- **Benchmarks** only on push to `main` and `workflow_dispatch` (avoids
  spending ~5 min of runner time per PR).
- Logs the runner's CPU model and AVX-512 feature flags before benching,
  so you can interpret the absolute numbers.
- Publishes BDN JSON output to GitHub Pages via
  `benchmark-action/github-action-benchmark@v1`. The chart at
  `https://pedrosakuma.github.io/palindrome/dev/bench/` shows the trend
  across commits.
- Alert threshold is intentionally loose (150%) because CI noise + CPU
  family swap can produce 30%+ swings between runs.

For reproducible numbers you would want a self-hosted runner pinned to
a specific CPU; the current setup is for a public showcase, not for
release-gating.

## Why these strategies and not others

- **AVX-512 with explicit intrinsics, not `Vector512<T>`**, because the
  generic type does not emit mask-register instructions or `vpermb`/
  `vpcompressw` — those are the actual reason AVX-512 is faster than
  "just wider vectors".
- **No allocating LINQ / `string.Reverse` variants** beyond the Naive
  baseline — they are an order of magnitude slower than even scalar
  two-pointer and uninteresting to optimise.
- **No NEON / WASM SIMD** — would be straightforward via `AdvSimd` /
  `PackedSimd` if a portable target were required, but out of scope.

## License

MIT — see [LICENSE](LICENSE).
