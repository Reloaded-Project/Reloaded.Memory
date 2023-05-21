using System.IO;
using FluentAssertions;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Utilities.Structures;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Streams;

public class LittleEndianBufferedStreamReaderTests
{
    [Fact]
    public void BaseStream_ShouldReturnSameBaseStream()
    {
        var buffer = new byte[10];
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.BaseStream.Should().BeSameAs(stream);
    }

    [Fact]
    public void BufferBytesAvailable_ShouldBeConsistent()
    {
        var buffer = new byte[10];
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.BufferedBytesAvailable.Should().Be(reader.BufferedBytesAvailable);
    }

    [Fact]
    public void ReadByte_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();

        littleEndianReader.Read(out byte first);
        first.Should().Be(0x12);
        littleEndianReader.ReadByte().Should().Be(0x34);
        littleEndianReader.ReadByte().Should().Be(0x56);
        littleEndianReader.ReadByte().Should().Be(0x78);
    }

    [Fact]
    public void ReadSByte_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();

        littleEndianReader.Read(out sbyte first);
        first.Should().Be(0x12);
        littleEndianReader.ReadSByte().Should().Be(0x34);
        littleEndianReader.ReadSByte().Should().Be(0x56);
        littleEndianReader.ReadSByte().Should().Be(0x78);
    }

    [Fact]
    public void ReadInt16_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x34, 0x12 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out short value);
        value.Should().Be(0x1234);
    }

    [Fact]
    public void ReadUInt16_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x34, 0x12 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out ushort value);
        value.Should().Be(0x1234);
    }

    [Fact]
    public void ReadInt32_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x78, 0x56, 0x34, 0x12 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out int value);
        value.Should().Be(0x12345678);
    }

    [Fact]
    public void ReadUInt32_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x78, 0x56, 0x34, 0x12 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out uint value);
        value.Should().Be(0x12345678);
    }

    [Fact]
    public void ReadInt64_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0xF1, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out long value);
        value.Should().Be(0x123456789ABCDEF1);
    }

    [Fact]
    public void ReadUInt64_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0xF1, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out ulong value);
        value.Should().Be(0x123456789ABCDEF1);
    }

    [Fact]
    public void ReadSingle_ShouldReturnCorrectValue()
    {
        // Single-precision floating-point number 1234.5678
        var buffer = new byte[] { 0x2B, 0x52, 0x9A, 0x44 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out float value);
        value.Should().BeApproximately(1234.5678f, 1e-6f);
    }

    [Fact]
    public void ReadDouble_ShouldReturnCorrectValue()
    {
        // Double-precision floating-point number 1234.5678
        var buffer = new byte[] { 0xAD, 0xFA, 0x5C, 0x6D, 0x45, 0x4A, 0x93, 0x40 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Read(out double value);
        value.Should().BeApproximately(1234.5678, 1e-6);
    }

    [Fact]
    public void PeekByte_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out byte value);
        value.Should().Be(0x01);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekSByte_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0xFE, 0xFF }; // -2, -1 in two's complement form
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out sbyte value);
        value.Should().Be(-2);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekInt16_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out short value);
        value.Should().Be(0x0304);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekUInt16_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out ushort value);
        value.Should().Be(0x0304);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekInt32_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out int value);
        value.Should().Be(0x01020304);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekUInt32_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out uint value);
        value.Should().Be(0x01020304U);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekInt64_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out long value);
        value.Should().Be(0x0102030405060708L);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekUInt64_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out ulong value);
        value.Should().Be(0x0102030405060708UL);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekSingle_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x79, 0xe9, 0xf6, 0x42 }; // equivalent to float 123.456
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out float value);
        value.Should().BeApproximately(123.456f, 0.0001f);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekDouble_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x77, 0xbe, 0x9f, 0x1a, 0x2f, 0xdd, 0x5e, 0x40 }; // equivalent to double 123.456
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var littleEndianReader = reader.AsLittleEndian();
        littleEndianReader.Peek(out double value);
        value.Should().BeApproximately(123.456, 0.0001);
        littleEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void ReadStruct_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        reader.AsLittleEndian().ReadStruct<UShortEx>(out var actual);
        actual.Should().Be((UShortEx)0x0201);
    }

    [Fact]
    public void PeekStruct_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        reader.AsLittleEndian().PeekStruct<UShortEx>(out var actual);
        actual.Should().Be((UShortEx)0x0201);
    }
}
