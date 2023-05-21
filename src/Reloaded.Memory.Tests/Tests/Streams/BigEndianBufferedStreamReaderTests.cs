using System.IO;
using FluentAssertions;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Utilities.Structures;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Streams;

public class BigEndianBufferedStreamReaderTests
{
    [Fact]
    public void BaseStream_ShouldReturnSameBaseStream()
    {
        var buffer = new byte[10];
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.BaseStream.Should().BeSameAs(stream);
    }

    [Fact]
    public void BufferBytesAvailable_ShouldBeConsistent()
    {
        var buffer = new byte[10];
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.BufferedBytesAvailable.Should().Be(reader.BufferedBytesAvailable);
    }

    [Fact]
    public void ReadByte_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();

        bigEndianReader.Read(out byte first);
        first.Should().Be(0x12);
        bigEndianReader.ReadByte().Should().Be(0x34);
        bigEndianReader.ReadByte().Should().Be(0x56);
        bigEndianReader.ReadByte().Should().Be(0x78);
    }

    [Fact]
    public void ReadSByte_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();

        bigEndianReader.Read(out sbyte first);
        first.Should().Be(0x12);
        bigEndianReader.ReadSByte().Should().Be(0x34);
        bigEndianReader.ReadSByte().Should().Be(0x56);
        bigEndianReader.ReadSByte().Should().Be(0x78);
    }

    [Fact]
    public void ReadInt16_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out short value);
        value.Should().Be(0x1234);
    }

    [Fact]
    public void ReadUInt16_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out ushort value);
        value.Should().Be(0x1234);
    }

    [Fact]
    public void ReadInt32_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out int value);
        value.Should().Be(0x12345678);
    }

    [Fact]
    public void ReadUInt32_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out uint value);
        value.Should().Be(0x12345678);
    }

    [Fact]
    public void ReadInt64_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF1 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out long value);
        value.Should().Be(0x123456789ABCDEF1);
    }

    [Fact]
    public void ReadUInt64_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF1 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out ulong value);
        value.Should().Be(0x123456789ABCDEF1);
    }

    [Fact]
    public void ReadSingle_ShouldReturnCorrectValue()
    {
        // Single-precision floating-point number for 1234.5678 in big endian is 0x449A522B
        var buffer = new byte[] { 0x44, 0x9A, 0x52, 0x2B };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out float value);
        value.Should().BeApproximately(1234.5678f, 1e-6f);
    }

    [Fact]
    public void ReadDouble_ShouldReturnCorrectValue()
    {
        // Double-precision floating-point number for 1234.5678 in big endian is 0x40934A456D5CFAAD
        var buffer = new byte[] { 0x40, 0x93, 0x4A, 0x45, 0x6D, 0x5C, 0xFA, 0xAD };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Read(out double value);
        value.Should().BeApproximately(1234.5678, 1e-6);
    }

    [Fact]
    public void PeekByte_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out byte value);
        value.Should().Be(0x01);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekSByte_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0xFE, 0xFF }; // -2, -1 in two's complement form
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out sbyte value);
        value.Should().Be(-2);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekInt16_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out short value);
        value.Should().Be(0x0102);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekUInt16_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out ushort value);
        value.Should().Be(0x0102);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekInt32_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out int value);
        value.Should().Be(0x01020304);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekUInt32_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out uint value);
        value.Should().Be(0x01020304U);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekInt64_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out long value);
        value.Should().Be(0x0102030405060708L);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekUInt64_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out ulong value);
        value.Should().Be(0x0102030405060708UL);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekSingle_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x42, 0xf6, 0xe9, 0x79 }; // equivalent to float 123.456 in big endian
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out float value);
        value.Should().BeApproximately(123.456f, 0.0001f);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void PeekDouble_ShouldReturnCorrectValueAndNotAdvanceStream()
    {
        var buffer = new byte[] { 0x40, 0x5e, 0xdd, 0x2f, 0x1a, 0x9f, 0xbe, 0x77 }; // equivalent to double 123.456 in big endian
        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);

        var bigEndianReader = reader.AsBigEndian();
        bigEndianReader.Peek(out double value);
        value.Should().BeApproximately(123.456, 0.0001);
        bigEndianReader.Position.Should().Be(0);
    }

    [Fact]
    public void ReadStruct_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        reader.AsBigEndian().ReadStruct<UShortEx>(out var actual);
        actual.Should().Be((UShortEx)0x0102);
    }

    [Fact]
    public void PeekStruct_ShouldReturnCorrectValue()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        reader.AsBigEndian().PeekStruct<UShortEx>(out var actual);
        actual.Should().Be((UShortEx)0x0102);
    }
}
