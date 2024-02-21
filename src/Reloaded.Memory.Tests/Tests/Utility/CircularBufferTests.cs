using System;
using FluentAssertions;
using Reloaded.Memory.Utilities;
using Xunit;
#pragma warning disable CS0618 // Type or member is obsolete

namespace Reloaded.Memory.Tests.Tests.Utility;

public unsafe class CircularBufferTests
{
    [Fact]
    public void Add_Byte()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        byte value = 0xAB;
        var result = circularBuffer.Add(value);

        circularBuffer.Current.Should().Be((nuint)buffer + 1);
        ((nuint)buffer).Should().Be(result);
        (*buffer).Should().Be(value);
    }

    [Fact]
    public void Add_Int32()
    {
        var buffer = stackalloc byte[8];
        var circularBuffer = new CircularBuffer((nuint)buffer, 8);

        var value = 0x12345678;
        var result = circularBuffer.Add(value);

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void Add_Int32_WithSource()
    {
        var buffer = stackalloc byte[8];
        var circularBuffer = new CircularBuffer((nuint)buffer, 8);

        var value = 0x12345678;
        var result = circularBuffer.Add(new Reloaded.Memory.Memory(), value);

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void Add_Ptr_Int32()
    {
        var buffer = stackalloc byte[8];
        var circularBuffer = new CircularBuffer((nuint)buffer, 8);

        var value = 0x12345678;
        var result = circularBuffer.Add((byte*)&value, sizeof(int));

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void Add_Ptr_Int32_WithSource()
    {
        var buffer = stackalloc byte[8];
        var circularBuffer = new CircularBuffer((nuint)buffer, 8);

        var value = 0x12345678;
        var result = circularBuffer.Add(new Reloaded.Memory.Memory(), (byte*)&value, sizeof(int));

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void Add_StartOfBuffer()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        var value = 0x12345678;
        // ReSharper disable once RedundantAssignment
        var result = circularBuffer.Add<byte>(0x0);
        result = circularBuffer.Add(value);

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void Add_StartOfBuffer_WithSource()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        var value = 0x12345678;
        // ReSharper disable once RedundantAssignment
        var result = circularBuffer.Add(new Reloaded.Memory.Memory(), (byte)0x0);
        result = circularBuffer.Add(new Reloaded.Memory.Memory(), value);

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void Add_Ptr_StartOfBuffer()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        var value = 0x12345678;
        // ReSharper disable once RedundantAssignment
        var result = circularBuffer.Add<byte>(0x0);
        result = circularBuffer.Add((byte*)&value, sizeof(int));

        circularBuffer.Current.Should().Be((nuint)buffer + 4);
        ((nuint)buffer).Should().Be(result);
        (*(int*)buffer).Should().Be(value);
    }

    [Fact]
    public void ItemTooLarge()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        var value = 0x1234567812345678L;
        var result = circularBuffer.Add(value);

        result.Should().Be(UIntPtr.Zero);
    }

    [Fact]
    public void CanItemFit_Yes()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        circularBuffer.CanItemFit<int>().Should().Be(CircularBuffer.ItemFit.Yes);
        circularBuffer.CanItemFit(sizeof(int)).Should().Be(CircularBuffer.ItemFit.Yes);
    }

    [Fact]
    public void CanItemFit_No()
    {
        var buffer = stackalloc byte[4];
        var circularBuffer = new CircularBuffer((nuint)buffer, 4);

        circularBuffer.CanItemFit<long>().Should().Be(CircularBuffer.ItemFit.No);
        circularBuffer.CanItemFit(sizeof(long)).Should().Be(CircularBuffer.ItemFit.No);
    }

    [Fact]
    public void CanItemFit_StartOfBuffer()
    {
        var buffer = stackalloc byte[8];
        var circularBuffer = new CircularBuffer((nuint)buffer, 8);

        var value = 0x12345678;
        circularBuffer.Add(value);
        circularBuffer.CanItemFit<int>().Should().Be(CircularBuffer.ItemFit.Yes);
        circularBuffer.CanItemFit(sizeof(int)).Should().Be(CircularBuffer.ItemFit.Yes);

        // Item can no longer fit due to off by one.
        circularBuffer.Add<byte>(0x0);
        circularBuffer.CanItemFit<int>().Should().Be(CircularBuffer.ItemFit.StartOfBuffer);
        circularBuffer.CanItemFit(sizeof(int)).Should().Be(CircularBuffer.ItemFit.StartOfBuffer);
    }
}
