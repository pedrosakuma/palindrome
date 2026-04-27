using System.Buffers;

namespace Palindrome.Core.Strategies;

/// <summary>
/// Normalizes into a pooled buffer (lower-cased, alphanumerics only),
/// then runs a tight two-pointer compare on the dense buffer. Useful as
/// a fair scalar baseline for the SIMD strategies, which require dense
/// data anyway.
/// </summary>
public sealed class NormalizedTwoPointerChecker : IPalindromeChecker
{
    public string Name => "Normalized + TwoPointer (scalar, pooled)";

    public bool IsPalindrome(ReadOnlySpan<char> text)
    {
        char[] rented = ArrayPool<char>.Shared.Rent(text.Length);
        try
        {
            int len = AsciiNormalizer.Normalize(text, rented);
            var span = rented.AsSpan(0, len);

            int left = 0;
            int right = len - 1;
            while (left < right)
            {
                if (span[left] != span[right])
                    return false;
                left++;
                right--;
            }
            return true;
        }
        finally
        {
            ArrayPool<char>.Shared.Return(rented);
        }
    }
}
