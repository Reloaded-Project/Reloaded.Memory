using System;
using FluentAssertions;
using Reloaded.Memory.Streams;
using Xunit;
using static System.BitConverter;
using static Reloaded.Memory.Utilities.Endian;

namespace Reloaded.Memory.Tests.Tests.Streams;

public unsafe class BigEndianWriterTests
{
    private void AssertEndianReverse<T>(T actual, T value) where T : unmanaged
    {
        var expected = !IsLittleEndian ? value : Reverse(value);
        actual.Should().Be(expected);
    }

    [Fact]
    public void Seek()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        writer.Seek(5);
        (writer.Ptr - ptr).Should().Be(5);
    }

    [Fact]
    public void WriteByte()
    {
        var ptr = stackalloc byte[1];
        var writer = new BigEndianWriter(ptr);
        byte value = 0x12;
        writer.Write(value);
        (*ptr).Should().Be(value);
    }

    [Fact]
    public void WriteByteAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        byte value = 0x12;
        writer.WriteAtOffset(value, 2);
        (*(ptr + 2)).Should().Be(value);
    }

    [Fact]
    public void WriteDouble()
    {
        var ptr = stackalloc byte[8];
        var writer = new BigEndianWriter(ptr);
        var value = 123.456789;
        writer.Write(value);
        var actual = *(double*)ptr;
        actual.Should().BeApproximately(!IsLittleEndian ? value : Reverse(value), 0.0000001);
    }

    [Fact]
    public void WriteDoubleAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        var value = 123.456789;
        writer.WriteAtOffset(value, 2);
        var actual = *(double*)(ptr + 2);
        actual.Should().BeApproximately(!IsLittleEndian ? value : Reverse(value), 0.0000001);
    }

    [Fact]
    public void WriteFloat()
    {
        var ptr = stackalloc byte[4];
        var writer = new BigEndianWriter(ptr);
        var value = 123.456f;
        writer.Write(value);
        var actual = *(float*)ptr;
        actual.Should().BeApproximately(!IsLittleEndian ? value : Reverse(value), 0.0001f);
    }

    [Fact]
    public void WriteFloatAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        var value = 123.456f;
        writer.WriteAtOffset(value, 2);
        var actual = *(float*)(ptr + 2);
        actual.Should().BeApproximately(!IsLittleEndian ? value : Reverse(value), 0.0001f);
    }

    [Fact]
    public void WriteInt()
    {
        var ptr = stackalloc byte[4];
        var writer = new BigEndianWriter(ptr);
        var value = 0x12345678;
        writer.Write(value);
        var actual = *(int*)ptr;
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteIntAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        var value = 0x12345678;
        writer.WriteAtOffset(value, 2);
        var actual = *(int*)(ptr + 2);
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteLong()
    {
        var ptr = stackalloc byte[8];
        var writer = new BigEndianWriter(ptr);
        var value = 0x123456789ABCDEF0;
        writer.Write(value);
        var actual = *(long*)ptr;
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteLongAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        var value = 0x123456789ABCDEF0;
        writer.WriteAtOffset(value, 2);
        var actual = *(long*)(ptr + 2);
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteShort()
    {
        var ptr = stackalloc byte[2];
        var writer = new BigEndianWriter(ptr);
        short value = 0x1234;
        writer.Write(value);
        var actual = *(short*)ptr;
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteShortAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        short value = 0x1234;
        writer.WriteAtOffset(value, 2);
        var actual = *(short*)(ptr + 2);
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteSByte()
    {
        var ptr = stackalloc byte[1];
        var writer = new BigEndianWriter(ptr);
        sbyte value = 0x12;
        writer.Write(value);
        var actual = *(sbyte*)ptr;
        actual.Should().Be(value);
    }

    [Fact]
    public void WriteSByteAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        sbyte value = 0x12;
        writer.WriteAtOffset(value, 2);
        var actual = *(sbyte*)(ptr + 2);
        actual.Should().Be(value);
    }

    [Fact]
    public void WriteUInt()
    {
        var ptr = stackalloc byte[4];
        var writer = new BigEndianWriter(ptr);
        uint value = 0x12345678;
        writer.Write(value);
        var actual = *(uint*)ptr;
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteUIntAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        uint value = 0x12345678;
        writer.WriteAtOffset(value, 2);
        var actual = *(uint*)(ptr + 2);
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteULong()
    {
        var ptr = stackalloc byte[8];
        var writer = new BigEndianWriter(ptr);
        ulong value = 0x123456789ABCDEF0;
        writer.Write(value);
        var actual = *(ulong*)ptr;
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteULongAtOffset()
    {
        var ptr = stackalloc byte[16];
        var writer = new BigEndianWriter(ptr);
        ulong value = 0x123456789ABCDEF0;
        writer.WriteAtOffset(value, 2);
        var actual = *(ulong*)(ptr + 2);
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteUShort()
    {
        var ptr = stackalloc byte[2];
        var writer = new BigEndianWriter(ptr);
        ushort value = 0x1234;
        writer.Write(value);
        var actual = *(ushort*)ptr;
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteUShortAtOffset()
    {
        var ptr = stackalloc byte[10];
        var writer = new BigEndianWriter(ptr);
        ushort value = 0x1234;
        writer.WriteAtOffset(value, 2);
        var actual = *(ushort*)(ptr + 2);
        AssertEndianReverse(actual, value);
    }

    [Fact]
    public void WriteSpan()
    {
        var ptr = stackalloc byte[4];
        var writer = new BigEndianWriter(ptr);
        byte[] value = { 0x12, 0x34, 0x56, 0x78 };
        var span = new Span<byte>(value);
        writer.Write(span);
        var actual = new Span<byte>(ptr, 4);
        actual.SequenceEqual(span).Should().BeTrue();
    }
}
