using FluentAssertions;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Tests.Utilities.Structures;
using Reloaded.Memory.Utilities;
using Xunit;

// ReSharper disable RedundantCast

namespace Reloaded.Memory.Tests.Tests.Pointers;

public unsafe class Ptr
{
    [Fact]
    public void Can_Create()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);

        ((nuint)pointer.Pointer).Should().NotBe((nuint)0);
        ((nuint)pointer.Pointer).Should().Be((nuint)(&value));
    }

    [Fact]
    public void Can_Equals()
    {
        var value = 42;
        Ptr<int> ptr1 = new(&value);
        Ptr<int> ptr2 = new(&value);
        ptr1.Should().Be(ptr2);
        (ptr1 == ptr2).Should().BeTrue();
    }

    [Fact]
    public void Can_InEquality()
    {
        var value1 = 42;
        var value2 = 24;
        Ptr<int> ptr1 = new(&value1);
        Ptr<int> ptr2 = new(&value2);
        ptr1.Should().NotBe(ptr2);
        (ptr1 != ptr2).Should().BeTrue();
    }

    [Fact]
    public void Can_AsRef()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);
        ref var valueRef = ref pointer.AsRef();
        valueRef.Should().Be(value);
    }

    [Fact]
    public void Can_GetSet()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);

        var read = pointer.Get();
        read.Should().Be(value);

        var newValue = 24;
        pointer.Set(in newValue);

        read = pointer.Get();
        read.Should().Be(newValue);
    }

    [Fact]
    public void Can_GetSetWithIndex()
    {
        var values = stackalloc int[3] { 1, 2, 3 };
        Ptr<int> pointer = new(&values[0]);

        var read = pointer.Get(1);
        read.Should().Be(2);

        var newValue = 24;
        pointer.Set(1, in newValue);

        read = pointer.Get(1);
        read.Should().Be(newValue);
    }

    [Fact]
    public void Can_AddSubtract()
    {
        var values = stackalloc int[3] { 1, 2, 3 };
        Ptr<int> pointer = new(&values[0]);

        Ptr<int> newPointer = pointer + 1;
        ((nuint)newPointer.Pointer).Should().Be((nuint)(&values[1]));

        newPointer -= 1;
        ((nuint)newPointer.Pointer).Should().Be((nuint)(&values[0]));
    }

    [Fact]
    public void Can_TrueFalse()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);
        if (pointer) { }
        else
            Assert.Fail("Pointer should be true!");

        Ptr<int> nullPointer = new(null);
        if (nullPointer)
            Assert.Fail("Pointer should be false!");
    }

    [Fact]
    public void Can_ToString()
    {
        var value = 42;
        Ptr<int> pointer = new(&value);

        var expectedString = $"Ptr<Int32> ({(ulong)&value:X})";
        pointer.ToString().Should().Be(expectedString);
    }

    [Fact]
    public void Can_AsRef_OnArrayOfPointers()
    {
        // Make array of four pointers.
        var a = 1;
        var b = 2;
        var c = 3;
        var d = 4;

        Ptr<int>[] arrayOfPointers = { new(&a), new(&b), new(&c), new(&d) };
        arrayOfPointers[0].AsRef().Should().Be(a);
        arrayOfPointers[1].AsRef().Should().Be(b);
        arrayOfPointers[2].AsRef().Should().Be(c);
        arrayOfPointers[3].AsRef().Should().Be(d);

        d = 99;
        arrayOfPointers[3].AsRef().Should().Be(99);
    }

    [Fact]
    public void Can_Increment_Decrement_Operators()
    {
        var intArray = stackalloc int[] { 1, 2, 3, 4, 5 };
        var ptr = new Ptr<int>(intArray);

        Assert.Equal(1, ptr.Get());
        ptr++;
        Assert.Equal(2, ptr.Get());
        ptr--;
        Assert.Equal(1, ptr.Get());
    }

    [Fact]
    public int* Can_Cast_Implicit()
    {
        var a = 5;
        return new Ptr<int>(&a);
    }

    [Fact]
    public Ptr<int> Can_Cast_Implicit_Back()
    {
        var a = 5;
        return &a;
    }

    [Fact]
    public void IsBlittable()
    {
        TypeInfo.ApproximateIsBlittable<Ptr<byte>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<sbyte>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<short>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<ushort>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<int>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<uint>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<long>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<ulong>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<nint>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<nuint>>().Should().BeTrue();
        TypeInfo.ApproximateIsBlittable<Ptr<RandomIntStruct>>().Should().BeTrue();
    }
}
