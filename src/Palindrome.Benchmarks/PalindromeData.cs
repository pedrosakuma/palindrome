namespace Palindrome.Benchmarks;

internal static class PalindromeData
{
    private const string Alphabet =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ,.!?";

    /// <summary>
    /// Builds a deterministic, exact-length string that is a palindrome
    /// after ASCII normalization (alphanumerics only, lower-cased).
    /// </summary>
    public static string Build(int length, int seed)
    {
        if (length <= 0) return string.Empty;

        var rng = new Random(seed);
        int half = length / 2;
        var first = new char[half];
        for (int i = 0; i < half; i++) first[i] = Alphabet[rng.Next(Alphabet.Length)];

        var normalized = new List<char>(half);
        foreach (var c in first)
            if (IsAlnum(c)) normalized.Add(char.ToLowerInvariant(c));

        var sb = new System.Text.StringBuilder(length);
        sb.Append(first);
        if (length % 2 == 1) sb.Append('x');
        for (int i = normalized.Count - 1; i >= 0; i--) sb.Append(normalized[i]);
        return sb.ToString();
    }

    private static bool IsAlnum(char c) =>
        (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
}
