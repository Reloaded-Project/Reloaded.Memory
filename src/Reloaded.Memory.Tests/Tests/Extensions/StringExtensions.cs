using System;
using System.Linq;
#if NET7_0_OR_GREATER
using System.Runtime.Intrinsics.X86;
#endif
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Internals.Algorithms;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class StringExtensions
{
    private static readonly Random _random = new();

    // TODO: The tests here could be more diverse. We're only scratching the surface of the barrel.

    /// <summary>
    ///     Test for hashing long strings, a quick baseline test.
    /// </summary>
    [Fact]
    public void HashCode_IsConsistent()
    {
        for (var x = 0; x < 128; x++)
        {
            var text = RandomString(x, RandomStringUpperWithEmoji(x));
            for (var y = 0; y < 5; y++)
            {
                #if NET7_0_OR_GREATER
                if (Avx2.IsSupported)
                    UnstableStringHash.UnstableHashAvx2(text).Should().Be(UnstableStringHash.UnstableHashAvx2(text));
                else if (Sse2.IsSupported)
                    UnstableStringHash.UnstableHashVec128(text).Should().Be(UnstableStringHash.UnstableHashVec128(text));
                else
                    UnstableStringHash.UnstableHashNonVector(text).Should().Be(UnstableStringHash.UnstableHashNonVector(text));
                #else
                text.GetHashCodeFast().Should().Be(text.AsSpan().UnstableHashNonVector());
                #endif
            }
        }
    }

    private static string RandomString(int length, string charSet) => new(Enumerable.Repeat(charSet, length)
        .Select(s => s[_random.Next(s.Length)]).ToArray());

    private static string RandomStringUpperWithEmoji(int length) => RandomString(length,
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ⚠️🚦🔺🏒😕🏞🖌🖕🌷☠⛩🍸👳🍠🚦📟💦🚏🌥🏪🌖😱");
}
