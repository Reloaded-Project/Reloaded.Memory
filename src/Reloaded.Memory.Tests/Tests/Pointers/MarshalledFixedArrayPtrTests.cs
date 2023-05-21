using System;
using FluentAssertions;
using Reloaded.Memory.Pointers;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Pointers;

public unsafe class MarshalledFixedArrayPtrTests
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
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, destinationArray.Length);
            fixedArrayPtr.CopyFrom(sourceArray.AsSpan(), sourceArray.Length);

            for (var x = 0; x < sourceArray.Length; x++)
                fixedArrayPtr.Get(x).Should().Be(sourceArray[x]);
        }
    }

    [Fact]
    public void CopyFrom_WithIndex()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = destinationArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, destinationArray.Length);
            fixedArrayPtr.CopyFrom(sourceArray.AsSpan(), 3, 1, 1);

            fixedArrayPtr.Get(0).Should().Be(0);
            fixedArrayPtr.Get(1).Should().Be(2);
            fixedArrayPtr.Get(2).Should().Be(3);
            fixedArrayPtr.Get(3).Should().Be(4);
            fixedArrayPtr.Get(4).Should().Be(0);
        }
    }

    [Fact]
    public void CopyFrom_WithSource()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = destinationArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, destinationArray.Length);
            fixedArrayPtr.CopyFrom(new Reloaded.Memory.Memory(), sourceArray.AsSpan(), sourceArray.Length);

            for (var x = 0; x < sourceArray.Length; x++)
                fixedArrayPtr.Get(x).Should().Be(sourceArray[x]);
        }
    }

    [Fact]
    public void CopyFrom_WithSource_WithIndex()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = destinationArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, destinationArray.Length);
            fixedArrayPtr.CopyFrom(new Reloaded.Memory.Memory(), sourceArray.AsSpan(), 3, 1, 1);

            fixedArrayPtr.Get(0).Should().Be(0);
            fixedArrayPtr.Get(1).Should().Be(2);
            fixedArrayPtr.Get(2).Should().Be(3);
            fixedArrayPtr.Get(3).Should().Be(4);
            fixedArrayPtr.Get(4).Should().Be(0);
        }
    }

    [Fact]
    public void CopyTo()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = sourceArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);
            fixedArrayPtr.CopyTo(destinationArray.AsSpan(), sourceArray.Length);

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
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);
            fixedArrayPtr.CopyTo(destinationArray.AsSpan(), 3, 1, 1);

            destinationArray[0].Should().Be(0);
            destinationArray[1].Should().Be(2);
            destinationArray[2].Should().Be(3);
            destinationArray[3].Should().Be(4);
            destinationArray[4].Should().Be(0);
        }
    }

    [Fact]
    public void CopyTo_WithSource()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = sourceArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);
            fixedArrayPtr.CopyTo(new Reloaded.Memory.Memory(), destinationArray.AsSpan(), sourceArray.Length);

            for (var x = 0; x < sourceArray.Length; x++)
            {
                destinationArray[x].Should().Be(sourceArray[x]);
            }
        }
    }

    [Fact]
    public void CopyTo_WithSource_WithIndex()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        var destinationArray = new int[5];

        fixed (int* ptr = sourceArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);
            fixedArrayPtr.CopyTo(new Reloaded.Memory.Memory(), destinationArray.AsSpan(), 3, 1, 1);

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
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);

            for (var x = 0; x < sourceArray.Length; x++)
                Assert.Equal(x, fixedArrayPtr.IndexOf(sourceArray[x]));

            Assert.Equal(-1, fixedArrayPtr.IndexOf(0));
            Assert.Equal(-1, fixedArrayPtr.IndexOf(6));
        }
    }

    [Fact]
    public void IndexOf_WithSource()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        fixed (int* ptr = sourceArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);

            for (var x = 0; x < sourceArray.Length; x++)
                Assert.Equal(x, fixedArrayPtr.IndexOf(new Reloaded.Memory.Memory(), sourceArray[x]));

            Assert.Equal(-1, fixedArrayPtr.IndexOf(new Reloaded.Memory.Memory(), 0));
            Assert.Equal(-1, fixedArrayPtr.IndexOf(new Reloaded.Memory.Memory(), 6));
        }
    }

    [Fact]
    public void Contains()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        fixed (int* ptr = sourceArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);
            foreach (var value in sourceArray)
                Assert.True(fixedArrayPtr.Contains(value));

            Assert.False(fixedArrayPtr.Contains(0));
            Assert.False(fixedArrayPtr.Contains(6));
        }
    }

    [Fact]
    public void Contains_WithSource()
    {
        var sourceArray = new[] { 1, 2, 3, 4, 5 };
        fixed (int* ptr = sourceArray)
        {
            var fixedArrayPtr = new MarshalledFixedArrayPtr<int>((byte*)ptr, sourceArray.Length);
            foreach (var value in sourceArray)
                Assert.True(fixedArrayPtr.Contains(new Reloaded.Memory.Memory(), value));

            Assert.False(fixedArrayPtr.Contains(new Reloaded.Memory.Memory(), 0));
            Assert.False(fixedArrayPtr.Contains(new Reloaded.Memory.Memory(), 6));
        }
    }
}
