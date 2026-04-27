namespace Palindrome.Core;

/// <summary>
/// Scalar normalizer: copies only ASCII alphanumerics into the destination
/// span, lower-casing letters along the way. Returns the written length.
/// </summary>
public static class AsciiNormalizer
{
    public static int Normalize(ReadOnlySpan<char> source, Span<char> destination)
    {
        int written = 0;
        for (int i = 0; i < source.Length; i++)
        {
            char c = source[i];
            if (AsciiHelpers.IsAlphanumeric(c))
            {
                destination[written++] = AsciiHelpers.ToLowerAsciiInvariant(c);
            }
        }
        return written;
    }
}
