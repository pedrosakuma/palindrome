namespace Palindrome.Core;

/// <summary>
/// Contract for palindrome checkers operating on ASCII text.
/// Implementations decide how (or whether) to skip non-letter/non-digit
/// symbols and how to perform case-insensitive comparison.
/// </summary>
public interface IPalindromeChecker
{
    /// <summary>Friendly name used by benchmarks/diagnostics.</summary>
    string Name { get; }

    /// <summary>
    /// Returns true when the text is a palindrome considering only ASCII
    /// letters and digits, case-insensitive.
    /// </summary>
    bool IsPalindrome(ReadOnlySpan<char> text);
}
