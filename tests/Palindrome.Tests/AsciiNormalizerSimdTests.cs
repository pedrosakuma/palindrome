using Palindrome.Core;

namespace Palindrome.Tests;

public class AsciiNormalizerSimdTests
{
    private static string ScalarNormalize(string s)
    {
        var dst = new char[s.Length];
        int n = AsciiNormalizer.Normalize(s, dst);
        return new string(dst, 0, n);
    }

    private static string Simd128Normalize(string s)
    {
        var dst = new char[s.Length];
        int n = AsciiNormalizerSimd.Normalize128(s, dst);
        return new string(dst, 0, n);
    }

    private static string Simd256Normalize(string s)
    {
        var dst = new char[s.Length];
        int n = AsciiNormalizerSimd.Normalize256(s, dst);
        return new string(dst, 0, n);
    }

    private static string Simd128x2Normalize(string s)
    {
        var dst = new char[s.Length];
        int n = AsciiNormalizerSimd.Normalize128x2(s, dst);
        return new string(dst, 0, n);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("Hello, World!")]
    [InlineData("A man, a plan, a canal: Panama")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnop")]
    [InlineData("!@#$%^&*()_+-=[]{}|;':\",./<>?`~")]
    [InlineData("Mix3d C4se W1th Symb0ls!!! 0123456789")]
    public void SimdMatchesScalar(string input)
    {
        var expected = ScalarNormalize(input);
        Assert.Equal(expected, Simd128Normalize(input));
        Assert.Equal(expected, Simd128x2Normalize(input));
        Assert.Equal(expected, Simd256Normalize(input));
    }

    [Fact]
    public void RandomInputs_AllAgree()
    {
        const string alphabet =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ,.!?;:'\"()[]{}-_+=/\\";
        var rng = new Random(2026);
        for (int len = 0; len <= 200; len++)
        {
            var arr = new char[len];
            for (int i = 0; i < len; i++) arr[i] = alphabet[rng.Next(alphabet.Length)];
            var s = new string(arr);
            var expected = ScalarNormalize(s);
            Assert.Equal(expected, Simd128Normalize(s));
            Assert.Equal(expected, Simd128x2Normalize(s));
            Assert.Equal(expected, Simd256Normalize(s));
        }
    }
}
