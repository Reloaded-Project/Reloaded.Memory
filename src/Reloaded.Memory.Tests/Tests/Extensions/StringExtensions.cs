using System;
using System.Linq;
using Reloaded.Memory.Extensions;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class StringExtensions
{
    private static readonly Random _random = new();

    // TODO: The tests here could be more diverse. We're only scratching the surface of the barrel.

    /// <summary>
    ///     Test for hashing long strings.
    ///     This is just a baseline quick test.
    /// </summary>
    [Fact]
    public void HashString()
    {
        for (var x = 0; x < 257; x++)
        {
            var text = RandomString(x, RandomStringUpperWithEmoji(x));
            var expected = text.GetHashCodeFast();

            for (var y = 0; y < 10; y++)
                Assert.Equal(expected, text.GetHashCodeFast());
        }
    }

    private static string RandomString(int length, string charSet) => new(Enumerable.Repeat(charSet, length)
        .Select(s => s[_random.Next(s.Length)]).ToArray());

    private static string RandomStringUpperWithEmoji(int length) => RandomString(length,
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ⚠️🚦🔺🏒😕🏞🖌🖕🌷☠⛩🍸👳🍠🚦📟💦🚏🌥🏪🌖😱");
}
