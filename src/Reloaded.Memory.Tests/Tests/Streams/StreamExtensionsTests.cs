using System;
using System.IO;
using System.Runtime.InteropServices;
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Streams;

public class StreamExtensionsTests
{
    [Fact]
    public void WriteReadUnmanagedTest()
    {
        using var memoryStream = new MemoryStream();
        var input = new UnmanagedStruct { A = 42, B = 3.14f };
        memoryStream.Write(input);
        memoryStream.Position = 0;
        memoryStream.Read(out UnmanagedStruct output);
        output.Should().Be(input);
    }

    [Fact]
    public void WriteReadMarshalledTest()
    {
        using var memoryStream = new MemoryStream();
        var input = new MarshalledStruct { A = 42, B = "Hello, World!" };
        memoryStream.WriteMarshalled(input);
        memoryStream.Position = 0;
        memoryStream.ReadMarshalled(out MarshalledStruct output);
        output.Should().Be(input);
    }

    [Fact]
    public void WriteReadUnmanagedArrayTest()
    {
        using var memoryStream = new MemoryStream();
        var input = new UnmanagedStruct[]
        {
            new() { A = 42, B = 3.14f },
            new() { A = 84, B = 6.28f },
        };
        memoryStream.Write(input.AsSpan());
        memoryStream.Position = 0;
        var output = new UnmanagedStruct[input.Length];
        memoryStream.Read(output.AsSpan());
        output.Should().Equal(input);
    }

    [Fact]
    public void WriteReadMarshalledArrayTest()
    {
        using var memoryStream = new MemoryStream();
        var input = new MarshalledStruct[]
        {
            new() { A = 42, B = "Hello, World!" },
            new() { A = 84, B = "Hi, Universe!" },
        };
        memoryStream.WriteMarshalled(input);
        memoryStream.Position = 0;
        var output = new MarshalledStruct[input.Length];
        memoryStream.ReadMarshalled(output);
        output.Should().Equal(input);
    }

    [Theory]
    [InlineData(0, 4, 0)]
    [InlineData(1, 4, 3)]
    [InlineData(2, 4, 2)]
    [InlineData(3, 4, 1)]
    [InlineData(4, 4, 0)]
    [InlineData(5, 4, 3)]
    public void AlignWithoutValue_ShouldAlignToAlignment(int initialPosition, int alignment, int expectedPadding)
    {
        // Arrange
        using var stream = new MemoryStream();
        stream.Position = initialPosition;

        // Act
        stream.AddPadding(alignment);

        // Assert
        (stream.Position % alignment).Should().Be(0);
        if (initialPosition != alignment)
            (stream.Length - initialPosition).Should().Be(expectedPadding);
    }

    [Theory]
    [InlineData(0, 4, 0, (byte)0xFF)]
    [InlineData(1, 4, 3, (byte)0xFF)]
    [InlineData(2, 4, 2, (byte)0xFF)]
    [InlineData(3, 4, 1, (byte)0xFF)]
    [InlineData(4, 4, 0, (byte)0xFF)]
    [InlineData(5, 4, 3, (byte)0xFF)]
    public void AlignWithValue_ShouldAlignToAlignmentAndPadWithValue(int initialPosition, int alignment, int expectedPadding, byte value)
    {
        // Arrange
        using var stream = new MemoryStream();
        stream.Position = initialPosition;

        // Act
        stream.AddPadding(value, alignment);

        // Assert
        (stream.Position % alignment).Should().Be(0);
        if (initialPosition != alignment)
            (stream.Length - initialPosition).Should().Be(expectedPadding);

        // Check that the padding bytes are set to the desired value
        stream.Position = initialPosition;
        for (int x = 0; x < expectedPadding; x++)
            stream.ReadByte().Should().Be(value);
    }

    public struct UnmanagedStruct
    {
        public int A;
        public float B;

        public override bool Equals(object obj) => obj is UnmanagedStruct other && A == other.A && B == other.B;
        public override int GetHashCode() => (A * 397) ^ B.GetHashCode();
    }

    public struct MarshalledStruct
    {
        public int A;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string B;

        public override bool Equals(object obj) => obj is MarshalledStruct other && A == other.A && B == other.B;
        public override int GetHashCode() => (A * 397) ^ B.GetHashCode();
    }
}
