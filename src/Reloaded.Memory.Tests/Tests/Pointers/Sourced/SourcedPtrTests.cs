using FluentAssertions;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Pointers.Sourced;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Pointers.Sourced;

public unsafe class SourcedPtrTests
{
    [Fact]
    public void Can_Create()
    {
        var value = 42;
        var memory = new Reloaded.Memory.Memory();
        var ptr = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value), memory);

        // ReSharper disable once RedundantCast
        ((nuint)ptr.Pointer.Pointer).Should().NotBe((nuint)0);
        ((nuint)ptr.Pointer.Pointer).Should().Be((nuint)(&value));
        ptr.Source.Should().Be(memory);
    }

    [Fact]
    public void Can_Equals()
    {
        var value = 42;
        var memory = new Reloaded.Memory.Memory();
        var ptr1 = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value), memory);
        var ptr2 = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value), memory);
        ptr1.Should().Be(ptr2);
        (ptr1 == ptr2).Should().BeTrue();
    }

    [Fact]
    public void Can_InEquality()
    {
        var value1 = 42;
        var value2 = 24;
        var memory = new Reloaded.Memory.Memory();
        var ptr1 = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value1), memory);
        var ptr2 = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value2), memory);
        ptr1.Should().NotBe(ptr2);
        (ptr1 != ptr2).Should().BeTrue();
    }

    [Fact]
    public void Can_AsRef()
    {
        var value = 42;
        var memory = new Reloaded.Memory.Memory();
        var ptr = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value), memory);

        ref var valueRef = ref ptr.AsRef();
        valueRef.Should().Be(value);
    }

    [Fact]
    public void Can_GetSet()
    {
        var value = 42;
        var newValue = 24;
        var memory = new Reloaded.Memory.Memory();
        var ptr = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(&value), memory);

        var read = ptr.Get();
        read.Should().Be(value);

        ptr.Set(in newValue);

        read = ptr.Get();
        read.Should().Be(newValue);
    }

    [Fact]
    public void Can_GetSetWithIndex()
    {
        var values = stackalloc int[3] { 1, 2, 3 };
        var memory = new Reloaded.Memory.Memory();
        var ptr = new SourcedPtr<int, Reloaded.Memory.Memory>(new Ptr<int>(values), memory);

        var read = ptr.Get(1);
        read.Should().Be(2);

        var newValue = 24;
        ptr.Set(1, in newValue);

        read = ptr.Get(1);
        read.Should().Be(newValue);
    }

    [Fact]
    public void Can_AddSubtract()
    {
        var values = stackalloc int[3] { 1, 2, 3 };
        Ptr<int> pointer = new(&values[0]);
        var memory = new Reloaded.Memory.Memory();
        SourcedPtr<int, Reloaded.Memory.Memory> sourcedPointer = new(pointer, memory);

        SourcedPtr<int, Reloaded.Memory.Memory> newPointer = sourcedPointer + 1;
        ((nuint)newPointer.Pointer.Pointer).Should().Be((nuint)(&values[1]));

        newPointer -= 1;
        ((nuint)newPointer.Pointer.Pointer).Should().Be((nuint)(&values[0]));
    }

    [Fact]
    public void Can_TrueFalse()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);
        Reloaded.Memory.Memory source = new();
        SourcedPtr<int, Reloaded.Memory.Memory> sourcedPointer = new(pointer, source);

        if (sourcedPointer) { }
        else
            Assert.Fail("SourcedPointer should be true!");

        Ptr<int> nullPointer = new(null);
        SourcedPtr<int, Reloaded.Memory.Memory> nullSourcedPointer = new(nullPointer, source);
        if (nullSourcedPointer)
            Assert.Fail("SourcedPointer should be false!");
    }

    [Fact]
    public void Can_ToString()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);
        Reloaded.Memory.Memory source = new();
        SourcedPtr<int, Reloaded.Memory.Memory> sourcedPointer = new(pointer, source);

        var expectedString = $"SourcedPtr<Int32, Memory> ({pointer})";
        sourcedPointer.ToString().Should().Be(expectedString);
    }

    [Fact]
    public void Can_AsRef_OnArrayOfSourcedPointers()
    {
        // Make array of four pointers.
        var a = 1;
        var b = 2;
        var c = 3;
        var d = 4;

        Reloaded.Memory.Memory source = new();

        SourcedPtr<int, Reloaded.Memory.Memory>[] arrayOfSourcedPointers =
        {
            new(new Ptr<int>(&a), source), new(new Ptr<int>(&b), source), new(new Ptr<int>(&c), source),
            new(new Ptr<int>(&d), source)
        };

        arrayOfSourcedPointers[0].AsRef().Should().Be(a);
        arrayOfSourcedPointers[1].AsRef().Should().Be(b);
        arrayOfSourcedPointers[2].AsRef().Should().Be(c);
        arrayOfSourcedPointers[3].AsRef().Should().Be(d);

        d = 99;
        arrayOfSourcedPointers[3].AsRef().Should().Be(99);
    }

    [Fact]
    public void Can_Increment_Decrement_Operators()
    {
        var intArray = stackalloc int[] { 1, 2, 3, 4, 5 };
        var ptr = new Ptr<int>(intArray);
        Reloaded.Memory.Memory source = new();
        SourcedPtr<int, Reloaded.Memory.Memory> sourcedPointer = new(ptr, source);

        Assert.Equal(1, sourcedPointer.Get());
        sourcedPointer++;
        Assert.Equal(2, sourcedPointer.Get());
        sourcedPointer--;
        Assert.Equal(1, sourcedPointer.Get());
    }
}
