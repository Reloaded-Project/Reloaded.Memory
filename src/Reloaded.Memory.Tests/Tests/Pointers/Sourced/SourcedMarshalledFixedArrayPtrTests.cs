using System;
using FluentAssertions;
using Reloaded.Memory.Pointers.Sourced;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Pointers.Sourced;

public unsafe class SourcedMarshalledFixedArrayPtrTests
{
    // Note: The items here don't need marshalling but will still use the marshalling APIs,
    // This simplifies the tests.
    // The actual marshalling APIs used under the hood are tested elsewhere.

    [Fact]
    public void CopyFrom()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = destinationArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, destinationArray.Length,
                    new Reloaded.Memory.Memory());
            sourcedFixedArrayPtr.CopyFrom(sourceArray.AsSpan(), sourceArray.Length);

            for (var x = 0; x < sourceArray.Length; x++)
                sourcedFixedArrayPtr.Get(x).Should().Be(sourceArray[x]);
        }
    }

    [Fact]
    public void CopyFrom_WithIndex()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = destinationArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, destinationArray.Length,
                    new Reloaded.Memory.Memory());
            sourcedFixedArrayPtr.CopyFrom(sourceArray.AsSpan(), 3, 1, 1);

            sourcedFixedArrayPtr.Get(0).Should().Be(0);
            sourcedFixedArrayPtr.Get(1).Should().Be(2);
            sourcedFixedArrayPtr.Get(2).Should().Be(3);
            sourcedFixedArrayPtr.Get(3).Should().Be(4);
            sourcedFixedArrayPtr.Get(4).Should().Be(0);
        }
    }

    [Fact]
    public void CopyTo()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = sourceArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, sourceArray.Length,
                    new Reloaded.Memory.Memory());
            sourcedFixedArrayPtr.CopyTo(destinationArray.AsSpan(), sourceArray.Length);

            for (var i = 0; i < sourceArray.Length; i++)
            {
                destinationArray[i].Should().Be(sourceArray[i]);
            }
        }
    }

    [Fact]
    public void CopyTo_WithIndex()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = sourceArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, sourceArray.Length,
                    new Reloaded.Memory.Memory());
            sourcedFixedArrayPtr.CopyTo(destinationArray.AsSpan(), 3, 1, 1);

            destinationArray[0].Should().Be(0);
            destinationArray[1].Should().Be(2);
            destinationArray[2].Should().Be(3);
            destinationArray[3].Should().Be(4);
            destinationArray[4].Should().Be(0);
        }
    }

    [Fact]
    public void IndexOf()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        fixed (int* ptr = sourceArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, sourceArray.Length,
                    new Reloaded.Memory.Memory());

            for (var x = 0; x < sourceArray.Length; x++)
                Assert.Equal(x, sourcedFixedArrayPtr.IndexOf(sourceArray[x]));

            Assert.Equal(-1, sourcedFixedArrayPtr.IndexOf(0));
            Assert.Equal(-1, sourcedFixedArrayPtr.IndexOf(6));
        }
    }

    [Fact]
    public void Contains()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        fixed (int* ptr = sourceArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, sourceArray.Length,
                    new Reloaded.Memory.Memory());
            foreach (var value in sourceArray)
                Assert.True(sourcedFixedArrayPtr.Contains(value));

            Assert.False(sourcedFixedArrayPtr.Contains(0));
            Assert.False(sourcedFixedArrayPtr.Contains(6));
        }
    }

    [Fact]
    public void Can_Enumerate()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        fixed (int* ptr = sourceArray)
        {
            var sourcedFixedArrayPtr =
                new SourcedMarshalledFixedArrayPtr<int, Reloaded.Memory.Memory>(ptr, sourceArray.Length,
                    new Reloaded.Memory.Memory());
            foreach (var value in sourcedFixedArrayPtr)
                Assert.True(sourcedFixedArrayPtr.Contains(value));
        }
    }
}
