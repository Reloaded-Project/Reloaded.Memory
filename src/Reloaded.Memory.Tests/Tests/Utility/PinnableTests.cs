using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Reloaded.Memory.Utilities;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Utility;

public unsafe class PinnableTests
{
    [Fact]
    public void Can_PinArray()
    {
        var array = new[] { 1, 2, 3, 4, 5 };

        using var pinnableArray = new Pinnable<int>(array);
        for (var x = 0; x < array.Length; x++)
            pinnableArray.Pointer[x].Should().Be(array[x]);
    }

    [Fact]
    public void Can_PinValueType()
    {
        var value = 42;

        using var pinnableValue = new Pinnable<int>(value);
        pinnableValue.Value.Should().Be(value);
    }

    [Fact]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void PinnableDisposeTest()
    {
        Pinnable<int> pinnable;

        using (pinnable = new Pinnable<int>(42))
            ((nuint)pinnable.Pointer).Should().NotBe((nuint)0x0);

        // The pointer should be set to null after disposing.
        ((nuint)pinnable.Pointer).Should().Be((nuint)0x0);
    }
}
