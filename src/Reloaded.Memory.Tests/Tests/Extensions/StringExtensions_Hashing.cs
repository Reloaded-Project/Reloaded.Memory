using System;
using System.Linq;
#if NET7_0_OR_GREATER
using System.Runtime.Intrinsics.X86;
#else
using Reloaded.Memory.Extensions;
#endif
using FluentAssertions;
using Reloaded.Memory.Internals.Algorithms;
using Xunit;
using static Reloaded.Memory.Tests.Utilities.StringGenerators;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class StringExtensions_Hashing
{
    // TODO: The tests here could be more diverse. We're only scratching the surface of the barrel.

    /// <summary>
    ///     Test for hashing long strings, a quick baseline test.
    /// </summary>
    [Fact]
    public void HashCode_IsConsistent()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringUpperWithEmoji(x);

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
}
