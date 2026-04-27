using Palindrome.Core;

namespace Palindrome.Tests;

public class PalindromeCheckersTests
{
    public static IEnumerable<object[]> Checkers =>
        PalindromeCheckers.All.Select(c => new object[] { c });

    [Theory]
    [MemberData(nameof(Checkers))]
    public void EmptyString_IsPalindrome(IPalindromeChecker checker)
    {
        Assert.True(checker.IsPalindrome(string.Empty));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void SingleChar_IsPalindrome(IPalindromeChecker checker)
    {
        Assert.True(checker.IsPalindrome("a"));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void ClassicPhrase_IsPalindrome(IPalindromeChecker checker)
    {
        Assert.True(checker.IsPalindrome("A man, a plan, a canal: Panama"));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void NonPalindrome_IsRejected(IPalindromeChecker checker)
    {
        Assert.False(checker.IsPalindrome("race a car"));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void NoLemonNoMelon_IsPalindrome(IPalindromeChecker checker)
    {
        Assert.True(checker.IsPalindrome("No lemon, no melon"));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void SymbolsOnly_IsPalindrome(IPalindromeChecker checker)
    {
        // Empty-after-normalization should be considered a palindrome.
        Assert.True(checker.IsPalindrome("!!!,,, ..."));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void DigitsAndLetters_AreCompared(IPalindromeChecker checker)
    {
        Assert.True(checker.IsPalindrome("1A2b2a1"));
        Assert.False(checker.IsPalindrome("1A2b3a1"));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void LongRandomPalindrome_IsAccepted(IPalindromeChecker checker)
    {
        var (text, expected) = BuildRandom(seed: 7, length: 4096, palindrome: true);
        Assert.Equal(expected, checker.IsPalindrome(text));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void LongRandomNonPalindrome_IsRejected(IPalindromeChecker checker)
    {
        var (text, expected) = BuildRandom(seed: 11, length: 4096, palindrome: false);
        Assert.Equal(expected, checker.IsPalindrome(text));
    }

    [Theory]
    [MemberData(nameof(Checkers))]
    public void OffByOneLengths_AllStrategiesAgree(IPalindromeChecker checker)
    {
        for (int len = 0; len <= 80; len++)
        {
            var (text, expected) = BuildRandom(seed: 100 + len, length: len, palindrome: true);
            Assert.True(checker.IsPalindrome(text), $"length={len}");
            Assert.True(expected);
        }
    }

    private static (string text, bool expected) BuildRandom(int seed, int length, bool palindrome)
    {
        var rng = new Random(seed);
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ,.!?";

        if (length == 0) return (string.Empty, true);

        int half = length / 2;
        var first = new char[half];
        for (int i = 0; i < half; i++) first[i] = alphabet[rng.Next(alphabet.Length)];

        var normalized = new List<char>();
        foreach (var c in first)
            if (IsAlnum(c)) normalized.Add(char.ToLowerInvariant(c));

        var sb = new System.Text.StringBuilder(length);
        sb.Append(first);
        if (length % 2 == 1) sb.Append('x');
        for (int i = normalized.Count - 1; i >= 0; i--) sb.Append(normalized[i]);

        if (!palindrome && normalized.Count > 0)
        {
            // Flip the last character of the mirrored half so the comparison fails.
            int idx = sb.Length - 1;
            sb[idx] = sb[idx] == 'a' ? 'b' : 'a';
        }

        return (sb.ToString(), palindrome || normalized.Count == 0);
    }

    private static bool IsAlnum(char c) =>
        (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
}
