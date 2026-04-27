namespace Palindrome.Core.Strategies;

/// <summary>
/// Classic in-place two-pointer scan with on-the-fly normalization.
/// Allocation-free and the typical "good enough" implementation.
/// </summary>
public sealed class TwoPointerChecker : IPalindromeChecker
{
    public string Name => "TwoPointer (scalar)";

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        int left = 0;
        int right = text.Length - 1;

        while (left < right)
        {
            char l = text[left];
            if (!AsciiHelpers.IsAlphanumeric(l))
            {
                left++;
                continue;
            }

            char r = text[right];
            if (!AsciiHelpers.IsAlphanumeric(r))
            {
                right--;
                continue;
            }

            if (AsciiHelpers.ToLowerAsciiInvariant(l) != AsciiHelpers.ToLowerAsciiInvariant(r))
                return false;

            left++;
            right--;
        }

        return true;
    }
}
