using System;
using System.Linq;

namespace Reloaded.Memory.Tests.Utilities;

/// <summary>
///     Utility methods for generating strings for tests.
/// </summary>
public static class StringGenerators
{
    private static readonly Random _random = new();

    internal static string RandomString(int length, string charSet) => new(Enumerable.Repeat(charSet, length)
        .Select(s => s[_random.Next(s.Length)]).ToArray());

    internal static string RandomStringAsciiUpperOnly(int length) => RandomString(length,
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

    internal static string RandomStringAsciiLowerOnly(int length) => RandomString(length,
        "abcdefghijklmnopqrstuvwxyz0123456789");

    internal static string RandomStringAsciiMixedCase(int length) => RandomString(length,
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

    internal static string RandomStringOfProblematicCharacters(int length) => RandomString(length,
        "éüñæßıIΔδЛл中日한✔️👍");
    internal static string RandomStringUpperWithEmoji(int length) => RandomString(length,
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ⚠️🚦🔺🏒😕🏞🖌🖕🌷☠⛩🍸👳🍠🚦📟💦🚏🌥🏪🌖😱");
}
