using System.Text;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Reference baseline: builds a normalized copy of the input and compares
/// it with its reversed form. Allocates two strings; intentionally simple.
/// </summary>
public sealed class NaiveAllocatingChecker : IPalindromeChecker
{
    public string Name => "Naive (allocating, normalize+reverse)";

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        var sb = new StringBuilder(text.Length);
        foreach (var c in text)
        {
            if (AsciiHelpers.IsAlphanumeric(c))
                sb.Append(AsciiHelpers.ToLowerAsciiInvariant(c));
        }

        var normalized = sb.ToString();
        var reversed = new string(normalized.Reverse().ToArray());
        return normalized == reversed;
    }
}
