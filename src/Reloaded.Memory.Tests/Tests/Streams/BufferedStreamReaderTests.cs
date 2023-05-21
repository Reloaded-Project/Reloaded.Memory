using System;
using System.IO;
using System.Runtime.InteropServices;
using FluentAssertions;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Utilities.Structures;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Streams;

public class BufferedStreamReaderTests
{
    [Fact]
    public void Construct_InitializesCorrectly()
    {
        using var stream = new MemoryStream();
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        reader.BaseStream.Should().BeSameAs(stream);
        reader.BufferedBytesAvailable.Should().Be(0);
        reader.CurrentBufferSize.Should().Be(0);
        reader.IsEndOfStream.Should().BeFalse();
    }

    [Fact]
    public void Read_ReadsCorrectly()
    {
        using var reader = new BufferedStreamReader<MemoryStream>(GenerateTestStream(4));
        var readByte = reader.Read<byte>();
        readByte.Should().Be(0);
        reader.Position.Should().Be(1);
    }

    [Fact]
    public void Peek_PeeksCorrectly()
    {
        using var reader = new BufferedStreamReader<MemoryStream>(GenerateTestStream(4));
        var peekByte = reader.Peek<byte>();
        peekByte.Should().Be(0);
        reader.Position.Should().Be(0); // Position shouldn't change after Peek
    }

    [Fact]
    public void Seek_SetsPositionCorrectly()
    {
        using var reader = new BufferedStreamReader<MemoryStream>(GenerateTestStream(4));
        reader.Seek(2, SeekOrigin.Begin);
        reader.Position.Should().Be(2);
        var readByte = reader.Read<byte>();
        readByte.Should().Be(2); // Read the third byte
    }

    [Fact]
    public void Seek_Begin_SetsPositionCorrectly()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        reader.Seek(25, SeekOrigin.Begin);
        reader.Position.Should().Be(25);
        var readByte = reader.Read<byte>();
        readByte.Should().Be(25);

        reader.Seek(0, SeekOrigin.Begin);
        reader.Position.Should().Be(0);
        readByte = reader.Read<byte>();
        readByte.Should().Be(0);
    }

    [Fact]
    public void Seek_Current_SetsPositionCorrectly()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        reader.Advance(25);
        reader.Position.Should().Be(25);
        var readByte = reader.Read<byte>();
        readByte.Should().Be(25);

        reader.Advance(10);
        reader.Position.Should().Be(36);
        readByte = reader.Read<byte>();
        readByte.Should().Be(36);

        reader.Advance(-20);
        reader.Position.Should().Be(17);
        readByte = reader.Read<byte>();
        readByte.Should().Be(17);
    }

    [Fact]
    public void Seek_End_SetsPositionCorrectly()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        reader.Seek(10, SeekOrigin.End);
        reader.Position.Should().Be(90);
        var readByte = reader.Read<byte>();
        readByte.Should().Be(90);

        reader.Seek(0, SeekOrigin.End);
        reader.Position.Should().Be(100);
    }

    [Fact]
    public void ReadMarshalled_ReadsCorrectData()
    {
        var testStruct = new MarshallingStruct();
        var size = Marshal.SizeOf(testStruct);
        var buffer = new byte[size];
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        Marshal.StructureToPtr(testStruct, handle.AddrOfPinnedObject(), false);
        handle.Free();

        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var readStruct = reader.ReadMarshalled<MarshallingStruct>();
        readStruct.Should().BeEquivalentTo(testStruct);
    }

    [Fact]
    public void PeekMarshalled_PeeksCorrectDataAndDoesNotAdvancePosition()
    {
        var testStruct = new MarshallingStruct();
        var size = Marshal.SizeOf(testStruct);
        var buffer = new byte[size];
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        Marshal.StructureToPtr(testStruct, handle.AddrOfPinnedObject(), false);
        handle.Free();

        using var stream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var peekStruct = reader.PeekMarshalled<MarshallingStruct>();
        peekStruct.Should().BeEquivalentTo(testStruct);
        reader.Position.Should().Be(0); // Position should not have advanced

        var readStruct = reader.ReadMarshalled<MarshallingStruct>();
        readStruct.Should().BeEquivalentTo(testStruct);
        reader.Position.Should().Be(size); // Position should have advanced
    }

    [Fact]
    public void ReadBytesRaw_ReadsCorrectData()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var offset = 25;
        var length = 10;
        Span<byte> data = stackalloc byte[length];
        reader.ReadBytesUnbuffered(offset, data);

        for (var x = 0; x < length; x++)
            data[x].Should().Be((byte)(offset + x));
    }

    [Fact]
    public void ReadBytesRaw_DoesNotChangePosition()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var offset = 25;
        var length = 10;
        Span<byte> data = stackalloc byte[length];

        var initialPosition = reader.Position;
        reader.ReadBytesUnbuffered(offset, data);
        var finalPosition = reader.Position;

        finalPosition.Should().Be(initialPosition); // Position should not have changed
    }

    [Fact]
    public unsafe void ReadRaw_ReadsCorrectData()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var length = 10;

        var result = reader.ReadRaw(length, out var available);

        available.Should().Be(length); // Check if correct amount of data is available
        for (var x = 0; x < length; x++)
            result[x].Should().Be((byte)x); // Check if the correct data is read
    }

    [Fact]
    public unsafe void ReadRaw_WithExistingBuffer_ReadsCorrectData()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var length = 10;

        // ReSharper disable once RedundantAssignment
        var result = reader.ReadRaw(10, out var available);
        result = reader.ReadRaw(length, out available);

        available.Should().Be(length); // Check if correct amount of data is available
        for (var x = 0; x < length; x++)
            result[x].Should().Be((byte)(x + 10)); // Check if the correct data is read
    }

    [Fact]
    public unsafe void ReadRaw_DoesNotExceedBufferSize()
    {
        using var stream = GenerateTestStream(100);
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var length = 200; // Requested length is more than the buffer size

        var result = reader.ReadRaw(length, out var available);

        available.Should().BeLessOrEqualTo(100); // Check if the available data does not exceed buffer size
        for (var x = 0; x < available; x++)
            result[x].Should().Be((byte)x); // Check if the correct data is read
    }

    [Fact]
    public unsafe void ReadRaw_WithGeneric_ReadsCorrectData()
    {
        using var stream = new MemoryStream(SetupIntArrayInByteArray(100));
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var numItems = 10;

        var result = reader.ReadRaw<int>(numItems, out var available);

        available.Should().Be(numItems); // Check if correct amount of data is available
        for (var x = 0; x < numItems; x++)
            result[x].Should().Be(x); // Check if the correct data is read
    }

    [Fact]
    public unsafe void ReadRaw_WithGeneric_DoesNotExceedBufferSize()
    {
        using var stream = new MemoryStream(SetupIntArrayInByteArray(100));
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var numItems = 200; // Requested items is more than the buffer size

        var result = reader.ReadRaw<int>(numItems, out var available);

        available.Should().BeLessOrEqualTo(100); // Check if the available data does not exceed buffer size
        for (var x = 0; x < available; x++)
            result[x].Should().Be(x); // Check if the correct data is read
    }

    [Fact]
    public unsafe void ReadRaw_WithGeneric_SeeksBackToLastMultipleOfT()
    {
        using var stream = new MemoryStream(SetupIntArrayInByteArray(100, new byte[399]));
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var numItems = 200; // Requested items is more than the buffer size

        var result = reader.ReadRaw<int>(numItems, out var available);

        available.Should().BeLessOrEqualTo(99);
        reader.Position.Should().Be(396); // seeked back to last multiple of T
        for (var x = 0; x < available; x++)
            result[x].Should().Be(x);
    }

    [Fact]
    public void ReadRaw_ReadsNoMoreThanSpanLength()
    {
        using var stream = new MemoryStream(SetupIntArrayInByteArray(100));
        using var reader = new BufferedStreamReader<MemoryStream>(stream);
        var span = new Span<int>(new int[200]); // Span length is more than the buffer size
        var itemsRead = reader.ReadRaw(span);

        itemsRead.Should().BeLessOrEqualTo(100); // Check if the number of items read is not more than the buffer size
        for (var x = 0; x < itemsRead; x++)
            span[x].Should().Be(x); // Check if the correct data is read into the span
    }

    [Fact]
    public void Read_AsLittleEndianStruct()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        var actual = reader.ReadLittleEndianStruct<UShortEx>();
        actual.Should().Be((UShortEx)0x0201);
    }

    [Fact]
    public void Read_AsBigEndianStruct()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        var actual = reader.ReadBigEndianStruct<UShortEx>();
        actual.Should().Be((UShortEx)0x0102);
    }

    [Fact]
    public void Peek_AsLittleEndianStruct()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        var actual = reader.PeekLittleEndianStruct<UShortEx>();
        actual.Should().Be((UShortEx)0x0201);
    }

    [Fact]
    public void Peek_AsBigEndianStruct()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        using var memoryStream = new MemoryStream(buffer);
        using var reader = new BufferedStreamReader<MemoryStream>(memoryStream);
        var actual = reader.PeekBigEndianStruct<UShortEx>();
        actual.Should().Be((UShortEx)0x0102);
    }

    private MemoryStream GenerateTestStream(int length)
    {
        var buffer = new byte[length];
        for (var x = 0; x < length; x++)
            buffer[x] = (byte)x;

        return new MemoryStream(buffer);
    }

    private static byte[] SetupIntArrayInByteArray(int numItems)
    {
        return SetupIntArrayInByteArray(numItems, new byte[numItems * sizeof(int)]);
    }

    private static byte[] SetupIntArrayInByteArray(int numItems, byte[] data)
    {
        // Arrange
        var buffer = new int[numItems];
        for (var x = 0; x < buffer.Length; x++)
            buffer[x] = x;

        Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
        return data;
    }
}
