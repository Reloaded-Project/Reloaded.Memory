using FluentAssertions;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Utilities.Structures;
using Xunit;
using static System.BitConverter;
using static Reloaded.Memory.Utilities.Endian;

namespace Reloaded.Memory.Tests.Tests.Streams;

public unsafe class LittleEndianReaderTests
{
    [Fact]
    public void ReadByte()
    {
        var ptr = stackalloc byte[1] {0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadByte();
        actual.Should().Be(0x12);
    }

    [Fact]
    public void ReadSByte()
    {
        var ptr = stackalloc sbyte[1] {-0x12};
        var reader = new LittleEndianReader((byte*)ptr);
        var actual = reader.ReadSByte();
        actual.Should().Be(-0x12);
    }

    [Fact]
    public void ReadShort()
    {
        var ptr = stackalloc byte[2] {0x34, 0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadShort();
        actual.Should().Be(IsLittleEndian ? (short)0x1234 : Reverse((short)0x1234));
    }

    [Fact]
    public void ReadUShort()
    {
        var ptr = stackalloc byte[2] {0x34, 0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadUShort();
        actual.Should().Be(IsLittleEndian ? (ushort)0x1234 : Reverse((ushort)0x1234));
    }

    [Fact]
    public void ReadInt()
    {
        var ptr = stackalloc byte[4] {0x78, 0x56, 0x34, 0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadInt();
        actual.Should().Be(IsLittleEndian ? 0x12345678 : Reverse(0x12345678));
    }

    [Fact]
    public void ReadUInt()
    {
        var ptr = stackalloc byte[4] {0x78, 0x56, 0x34, 0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadUInt();
        actual.Should().Be(IsLittleEndian ? 0x12345678u : Reverse(0x12345678u));
    }

    [Fact]
    public void ReadLong()
    {
        var ptr = stackalloc byte[8] {0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadLong();
        actual.Should().Be(IsLittleEndian ? 0x123456789ABCDEF0 : Reverse(0x123456789ABCDEF0));
    }

    [Fact]
    public void ReadULong()
    {
        var ptr = stackalloc byte[8] {0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadULong();
        actual.Should().Be(IsLittleEndian ? 0x123456789ABCDEF0ul : Reverse(0x123456789ABCDEF0ul));
    }

    [Fact]
    public void ReadFloat()
    {
        var ptr = stackalloc byte[4] {0xDB, 0x0F, 0x49, 0x40};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadFloat();
        actual.Should().Be(IsLittleEndian ? 3.1415927f : Reverse(3.1415927f));
    }

    [Fact]
    public void ReadDouble()
    {
        var ptr = stackalloc byte[8] {0x18, 0x2D, 0x44, 0x54, 0xFB, 0x21, 0x09, 0x40};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadDouble();
        actual.Should().Be(IsLittleEndian ? 3.1415926535897931 : Reverse(3.1415926535897931));
    }

    // Add similar tests for each ReadAtOffset method.

    [Fact]
    public void ReadByteAtOffset()
    {
        var ptr = stackalloc byte[2] {0x12, 0x34};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadByteAtOffset(1);
        actual.Should().Be(0x34);
    }

    [Fact]
    public void ReadSByteAtOffset()
    {
        var ptr = stackalloc sbyte[2] { -0x12, -0x34 };
        var reader = new LittleEndianReader((byte*)ptr);
        var actual = reader.ReadSByteAtOffset(1);
        actual.Should().Be(-0x34);
    }

    [Fact]
    public void ReadShortAtOffset()
    {
        var ptr = stackalloc byte[4] {0x12, 0x34, 0x56, 0x78};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadShortAtOffset(2);
        actual.Should().Be(IsLittleEndian ? (short)0x7856 : Reverse((short)0x7856));
    }

    [Fact]
    public void ReadUShortAtOffset()
    {
        var ptr = stackalloc byte[4] {0x12, 0x34, 0x56, 0x78};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadUShortAtOffset(2);
        actual.Should().Be(IsLittleEndian ? (ushort)0x7856 : Reverse((ushort)0x7856));
    }

    [Fact]
    public void ReadIntAtOffset()
    {
        var ptr = stackalloc byte[8] {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadIntAtOffset(4);
        actual.Should().Be(IsLittleEndian ? unchecked((int)0xEFCDAB90) : Reverse(unchecked((int)0xEFCDAB90)));
    }

    [Fact]
    public void ReadUIntAtOffset()
    {
        var ptr = stackalloc byte[8] {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadUIntAtOffset(4);
        actual.Should().Be(IsLittleEndian ? 0xEFCDAB90u : Reverse(0xEFCDAB90u));
    }

    [Fact]
    public void ReadLongAtOffset()
    {
        var ptr = stackalloc byte[16] {0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadLongAtOffset(8);
        actual.Should().Be(IsLittleEndian ? unchecked((long)0xFEDCBA9876543210) : Reverse(unchecked((long)0xFEDCBA9876543210)));
    }

    [Fact]
    public void ReadULongAtOffset()
    {
        var ptr = stackalloc byte[16] {0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadULongAtOffset(8);
        actual.Should().Be(IsLittleEndian ? 0xFEDCBA9876543210u : Reverse(0xFEDCBA9876543210u));
    }

    [Fact]
    public void ReadFloatAtOffset()
    {
        var ptr = stackalloc byte[8] {0xDB, 0x0F, 0x49, 0x40, 0x3F, 0xE0, 0xE9, 0x3E};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadFloatAtOffset(4);
        actual.Should().Be(IsLittleEndian ? 0.456789f : Reverse(0.456789f));
    }

    [Fact]
    public void ReadDoubleAtOffset()
    {
        var ptr = stackalloc byte[16] {0x18, 0x2D, 0x44, 0x54, 0xFB, 0x21, 0x09, 0x40, 0x18, 0x2d, 0x44, 0x54, 0xfb, 0x21, 0x09, 0x40};
        var reader = new LittleEndianReader(ptr);
        var actual = reader.ReadDoubleAtOffset(8);
        actual.Should().Be(IsLittleEndian ? 3.1415926535897931 : Reverse(3.1415926535897931));
    }

    [Fact]
    public void ReadStructViaPointer()
    {
        var ptr = stackalloc byte[10];
        var value = new RandomIntStruct();
        var reader = new LittleEndianReader(ptr);
        var writer = new LittleEndianWriter(ptr);
        writer.Write(value);
        var newValue = reader.Read<RandomIntStruct>();
        newValue.Should().Be(value);
    }

    [Fact]
    public void Seek()
    {
        var ptr = stackalloc byte[4] {0x12, 0x34, 0x56, 0x78};
        var reader = new LittleEndianReader(ptr);
        reader.Seek(2);
        var actual = reader.ReadByte();
        actual.Should().Be(0x56);
    }
}
