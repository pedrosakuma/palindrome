using BenchmarkDotNet.Running;
using Palindrome.Benchmarks;
using Palindrome.Core;

if (args.Length > 0 && args[0].Equals("info", StringComparison.OrdinalIgnoreCase))
{
    PrintCapabilities();
    return;
}

if (args.Length > 0 && args[0].Equals("smoke", StringComparison.OrdinalIgnoreCase))
{
    RunSmoke();
    return;
}

BenchmarkSwitcher.FromAssembly(typeof(PalindromeBenchmarks).Assembly).Run(args);

static void PrintCapabilities()
{
    Console.WriteLine($"Vector<T>.IsHardwareAccelerated  : {System.Numerics.Vector.IsHardwareAccelerated}");
    Console.WriteLine($"Vector<ushort>.Count             : {System.Numerics.Vector<ushort>.Count}");
    Console.WriteLine($"Vector128.IsHardwareAccelerated  : {System.Runtime.Intrinsics.Vector128.IsHardwareAccelerated}");
    Console.WriteLine($"Vector256.IsHardwareAccelerated  : {System.Runtime.Intrinsics.Vector256.IsHardwareAccelerated}");
    Console.WriteLine($"Vector512.IsHardwareAccelerated  : {System.Runtime.Intrinsics.Vector512.IsHardwareAccelerated}");
}

static void RunSmoke()
{
    string[] cases =
    {
        "A man, a plan, a canal: Panama",
        "race a car",
        "No lemon, no melon",
        string.Empty,
        "a",
    };

    foreach (var checker in PalindromeCheckers.All)
    {
        Console.WriteLine($"-- {checker.Name}");
        foreach (var c in cases)
            Console.WriteLine($"   {checker.IsPalindrome(c),-5}  '{c}'");
    }
}
