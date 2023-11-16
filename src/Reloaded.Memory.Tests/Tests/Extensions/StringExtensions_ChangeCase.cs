#if NET7_0_OR_GREATER
using System.Linq;
using System.Runtime.Intrinsics.X86;
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Internals.Algorithms;
using Xunit;
using static Reloaded.Memory.Tests.Utilities.StringGenerators;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class StringExtensions_ChangeCase
{
    /// <summary>
    ///     Test for hashing long strings, a quick baseline test.
    /// </summary>
    [Fact]
    public void ChangeCase_WithAsciiStrings_IsConsistentWithRuntime()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            for (var y = 0; y < 5; y++)
            {
                // Make some random text.
                var text = RandomStringAsciiMixedCase(x);

                // Convert with our methods, and assert it's the same as the runtime.
                text.ToLowerInvariantFast().Should().Be(text.ToLowerInvariant());
                text.ToUpperInvariantFast().Should().Be(text.ToUpperInvariant());
            }
        }
    }

    /// <summary>
    ///     Test for hashing long strings, a quick baseline test.
    /// </summary>
    [Fact]
    public void ChangeCase_WithMixedStrings_IsConsistentWithRuntime_WhenInvariant()
    {
        for (var x = 0; x < 34; x++) // 1 more than Vec256 * 2 can store
        {
            var remainder = x - (x / 2);
            for (var y = 0; y < 10; y++)
            {
                // Make some random text.
                var text = RandomStringAsciiMixedCase(x / 2)
                    .Concat(RandomStringOfProblematicCharacters(remainder))
                    .ToString();

                // Convert with our methods, and assert it's the same as the runtime.
                text!.ToLowerInvariantFast().Should().Be(text!.ToLowerInvariant());
                text.ToUpperInvariantFast().Should().Be(text.ToUpperInvariant());
            }
        }
    }
}
#endif
