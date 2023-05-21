using System;
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class SpanExtensions_Replace
{
    [Fact]
    public void Replace_EmptyInput_ShouldReturnEmpty()
    {
        // Arrange
        Span<char> data = Array.Empty<char>();
        Span<char> buffer = Array.Empty<char>();

        // Act
        Span<char> result = data.Replace('a', 'b', buffer);

        // Assert
        result.Length.Should().Be(0);
    }

    [Theory]
    // Long inputs for SIMD
    [InlineData("abcdefghabcdefghabcdefghabcdefgh", 'x', 'y', "abcdefghabcdefghabcdefghabcdefgh")]
    [InlineData("axcexghaxcexghaxcexghaxcexgh", 'x', 'y', "ayceyghayceyghayceyghayceygh")]
    [InlineData("xaxcxgxaxcxgxaxcxgxaxcxgx", 'x', 'y', "yaycygyaycygyaycygyaycygy")]
    [InlineData("axcexghaxcexghaxcexghaxcexghxaxcxgxaxcxgxaxcxgxaxcxgx", 'x', 'y',
        "ayceyghayceyghayceyghayceyghyaycygyaycygyaycygyaycygy")]
    // Shorter inputs for non-SIMD
    [InlineData("abcdefgh", 'x', 'y', "abcdefgh")]
    [InlineData("axcexgh", 'x', 'y', "ayceygh")]
    [InlineData("xaxcxgx", 'x', 'y', "yaycygy")]
    public void Replace_CharInput_ShouldReplaceCorrectly(string input, char oldValue, char newValue,
        string expectedResult)
    {
        // Arrange
        Span<char> data = new char[input.Length];
        Span<char> buffer = new char[input.Length];
        input.AsSpan().CopyTo(data);

        // Act
        Span<char> result = data.Replace(oldValue, newValue, buffer);

        // Assert
        result.ToString().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 9, 10, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
    [InlineData(new byte[] { 1, 9, 3, 4, 5, 6, 7, 8 }, 9, 10, new byte[] { 1, 10, 3, 4, 5, 6, 7, 8 })]
    [InlineData(new byte[] { 9, 1, 9, 4, 5, 6, 7, 8 }, 9, 10, new byte[] { 10, 1, 10, 4, 5, 6, 7, 8 })]
    public void Replace_ByteInput_ShouldReplaceCorrectly(byte[] input, byte oldValue, byte newValue,
        byte[] expectedResult)
    {
        // Arrange
        Span<byte> data = input.AsSpan();
        Span<byte> buffer = new byte[input.Length];

        // Act
        Span<byte> result = data.Replace(oldValue, newValue, buffer);

        // Assert
        result.ToArray().Should().BeEquivalentTo(expectedResult);
    }
}
