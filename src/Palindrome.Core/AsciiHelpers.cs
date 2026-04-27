using System.Runtime.CompilerServices;

namespace Palindrome.Core;

/// <summary>
/// Branch-light helpers for ASCII normalization.
/// </summary>
public static class AsciiHelpers
{
    /// <summary>True for [A-Za-z0-9].</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlphanumeric(char c)
    {
        // Unsigned trick: characters outside the basic alphanumeric ranges
        // become large unsigned values and short-circuit cheaply.
        uint u = c;
        return (uint)(u - '0') <= 9
            || (uint)((u | 0x20) - 'a') <= 25;
    }

    /// <summary>Lower-case an ASCII letter; returns the original char otherwise.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToLowerAsciiInvariant(char c)
    {
        // 'A'..'Z' -> 'a'..'z'; everything else unchanged.
        return (uint)(c - 'A') <= 25u ? (char)(c | 0x20) : c;
    }
}
