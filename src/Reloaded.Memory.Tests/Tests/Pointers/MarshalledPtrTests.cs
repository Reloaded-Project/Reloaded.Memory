using System.Runtime.InteropServices;
using FluentAssertions;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Tests.Utilities.Structures;
using Xunit;

#pragma warning disable CS8500

// ReSharper disable RedundantCast

namespace Reloaded.Memory.Tests.Tests.Pointers;

public unsafe class MarshalledPtrTests
{
    [Fact]
    public void Can_Create()
    {
        var sample = new MarshallingStruct();
        MarshalledPtr<MarshallingStruct> ptr = new((byte*)&sample);
        ((nuint)ptr.Pointer).Should().NotBe((nuint)0);
    }

    [Fact]
    public void Can_Equals()
    {
        var sample = new MarshallingStruct();
        MarshalledPtr<MarshallingStruct> ptr1 = new((byte*)&sample);
        MarshalledPtr<MarshallingStruct> ptr2 = new((byte*)&sample);
        ptr1.Should().Be(ptr2);
        (ptr1 == ptr2).Should().BeTrue();
    }

    [Fact]
    public void Can_Inequality()
    {
        var sample1 = new MarshallingStruct();
        var sample2 = new MarshallingStruct();
        MarshalledPtr<MarshallingStruct> ptr1 = new((byte*)&sample1);
        MarshalledPtr<MarshallingStruct> ptr2 = new((byte*)&sample2);

        ptr1.Should().NotBe(ptr2);
        (ptr1 != ptr2).Should().BeTrue();
    }

    [Fact]
    public void Can_GetSet()
    {
        var sample = new MarshallingStruct();
        var size = Marshal.SizeOf<MarshallingStruct>();
        var buffer = stackalloc byte[size];
        var memory = new Reloaded.Memory.Memory();
        memory.WriteWithMarshalling((nuint)buffer, sample);

        MarshalledPtr<MarshallingStruct> ptr = new(buffer);
        MarshallingStruct read = ptr.Get();
        read.Should().Be(sample);

        var newValue = new MarshallingStruct();
        ptr.Set(in newValue);

        read = ptr.Get();
        read.Should().Be(newValue);
    }

    [Fact]
    public void Can_GetSetWithIndex()
    {
        var samples = new MarshallingStruct[] { new(), new(), new() };
        var sizeOfStruct = Marshal.SizeOf<MarshallingStruct>();
        var totalSize = sizeOfStruct * samples.Length;
        var memory = new Reloaded.Memory.Memory();

        var buffer = stackalloc byte[totalSize];
        for (var x = 0; x < samples.Length; x++)
            memory.WriteWithMarshalling((nuint)(buffer + x * sizeOfStruct), samples[x]);

        MarshalledPtr<MarshallingStruct> ptr = new(buffer);
        for (var x = 0; x < samples.Length; x++)
        {
            MarshallingStruct read = ptr.Get(x);
            read.Should().Be(samples[x]);

            var newValue = new MarshallingStruct();
            ptr.Set(x, in newValue);

            read = ptr.Get(x);
            read.Should().Be(newValue);
        }
    }

    [Fact]
    public void Can_AddSubtract()
    {
        MarshalledPtr<MarshallingStruct> ptr = new((byte*)0x1000);
        MarshalledPtr<MarshallingStruct> newPtr = ptr + 1;
        newPtr.ElementSize.Should().NotBe(0);
        ((nuint)newPtr.Pointer).Should().Be((nuint)ptr.Pointer + (uint)Marshal.SizeOf<MarshallingStruct>());

        newPtr -= 1;
        newPtr.ElementSize.Should().NotBe(0);
        ((nuint)newPtr.Pointer).Should().Be((nuint)ptr.Pointer);
    }

    [Fact]
    public void MarshalledPtr_Increment_Decrement_Operators_Test()
    {
        MarshalledPtr<MarshallingStruct> ptr = new((byte*)0x1000);
        MarshalledPtr<MarshallingStruct> newPtr = ptr;

        // ++ operator
        newPtr++;
        newPtr.ElementSize.Should().NotBe(0);
        ((nuint)newPtr.Pointer).Should().Be((nuint)ptr.Pointer + (uint)Marshal.SizeOf<MarshallingStruct>());

        // -- operator
        newPtr--;
        newPtr.ElementSize.Should().NotBe(0);
        ((nuint)newPtr.Pointer).Should().Be((nuint)ptr.Pointer);
    }

    [Fact]
    public void Can_TrueFalse()
    {
        var value = 42;
        MarshalledPtr<MarshallingStruct> pointer = new((byte*)&value);
        if (pointer) { }
        else
            Assert.Fail("Pointer should be true!");

        MarshalledPtr<MarshallingStruct> nullPointer = new(null);
        if (nullPointer)
            Assert.Fail("Pointer should be false!");
    }
}
