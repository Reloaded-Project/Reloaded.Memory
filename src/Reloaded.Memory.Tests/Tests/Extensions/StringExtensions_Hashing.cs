#if NET7_0_OR_GREATER
using System.Runtime.Intrinsics.X86;
using Reloaded.Memory.Internals.Algorithms;
#else
using Reloaded.Memory.Internals.Algorithms;
#endif
using System;
using FluentAssertions;
using Reloaded.Memory.Extensions;
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
                    UnstableStringHash.UnstableHashVec256(text).Should().Be(UnstableStringHash.UnstableHashVec256(text));
                if (Sse2.IsSupported)
                    UnstableStringHash.UnstableHashVec128(text).Should().Be(UnstableStringHash.UnstableHashVec128(text));

                UnstableStringHash.UnstableHashNonVector(text).Should().Be(UnstableStringHash.UnstableHashNonVector(text));
                #else
                text.GetHashCodeFast().Should().Be(text.AsSpan().GetHashCodeFast());
                #endif
            }
        }
    }

#if NET7_0_OR_GREATER
    /// <summary>
    ///     Test for hashing strings in a case insensitive fashion.
    ///
    ///     This test works by testing hashing a mixed case string against an already lower case string,
    ///     the results should be the same.
    /// </summary>
    [Fact]
    public void HashCodeLower_IsCaseInsensitive_UsingAvx2()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringAsciiMixedCase(x);
                var lowerInvariantText = text.ToLowerInvariantFast();
                UnstableStringHashLower.UnstableHashVec256Lower(text).Should().Be(UnstableStringHashLower.UnstableHashVec256Lower(lowerInvariantText));
            }
        }
    }

    /// <summary>
    ///     Test for hashing strings in a case insensitive fashion.
    ///
    ///     This test works by testing hashing a mixed case string against an already lower case string,
    ///     the results should be the same.
    /// </summary>
    [Fact]
    public void HashCodeLower_IsCaseInsensitive_UsingVec128()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringAsciiMixedCase(x);
                var lowerInvariantText = text.ToLowerInvariantFast();
                UnstableStringHashLower.UnstableHashVec128Lower(text).Should().Be(UnstableStringHashLower.UnstableHashVec128Lower(lowerInvariantText));
            }
        }
    }
#endif

    /// <summary>
    ///     Test for hashing strings in a case insensitive fashion.
    ///
    ///     This test works by testing hashing a mixed case string against an already lower case string,
    ///     the results should be the same.
    /// </summary>
    [Fact]
    public void HashCodeLower_IsCaseInsensitive_UsingNonVector32()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringAsciiMixedCase(x);
                var lowerInvariantText = text.ToLowerInvariant();
                text.AsSpan().UnstableHashNonVectorLower32().Should().Be(lowerInvariantText.AsSpan().UnstableHashNonVectorLower32());
            }
        }
    }

    /// <summary>
    ///     Test for hashing strings in a case insensitive fashion.
    ///
    ///     This test works by testing hashing a mixed case string against an already lower case string,
    ///     the results should be the same.
    /// </summary>
    [Fact]
    public void HashCodeLower_IsCaseInsensitive_UsingNonVector64()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringAsciiMixedCase(x);
                var lowerInvariantText = text.ToLowerInvariant();
                text.AsSpan().UnstableHashNonVectorLower64().Should().Be(lowerInvariantText.AsSpan().UnstableHashNonVectorLower64());
            }
        }
    }

    /// <summary>
    ///     Test for hashing strings in a case insensitive fashion.
    ///
    ///     This test works by testing hashing a mixed case string against an already lower case string,
    ///     the results should be the same.
    /// </summary>
    [Fact]
    public void HashCodeLower_IsCaseInsensitive_UsingLegacyFallback()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringAsciiMixedCase(x);
                var lowerInvariantText = text.ToLowerInvariant();
                text.GetHashCodeLowerFast().Should().Be(lowerInvariantText.AsSpan().GetHashCodeLowerFast());
            }
        }
    }

    /// <summary>
    ///     Test for hashing strings in a case insensitive fashion.
    ///
    ///     This test works by testing hashing a mixed case string against an already lower case string,
    ///     the results should be the same.
    /// </summary>
    [Fact]
    public void HashCodeLower_IsCaseInsensitive_UsingSlow()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                var text = RandomStringAsciiMixedCase(x);
                var lowerInvariantText = text.ToLowerInvariant();
                text.AsSpan().GetHashCodeUnstableLowerSlow().Should().Be(lowerInvariantText.AsSpan().GetHashCodeUnstableLowerSlow());
            }
        }
    }
}
