using System;
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class SpanExtensions_SliceFast
{
    [Fact]
    public void SliceFast_StartIndex_ShouldSliceCorrectly()
    {
        // Arrange
        Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        Span<int> result = data.SliceFast(5);

        // Assert
        result.ToArray().Should().Equal(6, 7, 8, 9, 10);
    }

    [Fact]
    public void SliceFast_ReadOnlySpanStartIndex_ShouldSliceCorrectly()
    {
        // Arrange
        ReadOnlySpan<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        ReadOnlySpan<int> result = data.SliceFast(5);

        // Assert
        result.ToArray().Should().Equal(6, 7, 8, 9, 10);
    }

    [Fact]
    public void SliceFast_StartIndexAndLength_ShouldSliceCorrectly()
    {
        // Arrange
        Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        Span<int> result = data.SliceFast(3, 4);

        // Assert
        result.ToArray().Should().Equal(4, 5, 6, 7);
    }

    [Fact]
    public void SliceFast_ReadOnlySpanStartIndexAndLength_ShouldSliceCorrectly()
    {
        // Arrange
        ReadOnlySpan<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        ReadOnlySpan<int> result = data.SliceFast(3, 4);

        // Assert
        result.ToArray().Should().Equal(4, 5, 6, 7);
    }

    [Fact]
    public void SliceFast_Range_ShouldSliceCorrectly()
    {
        // Arrange
        Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        Span<int> result = data.SliceFast(3..7);

        // Assert
        result.ToArray().Should().Equal(4, 5, 6, 7);
    }

    [Fact]
    public void SliceFast_ReadOnlySpanRange_ShouldSliceCorrectly()
    {
        // Arrange
        ReadOnlySpan<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        ReadOnlySpan<int> result = data.SliceFast(3..7);

        // Assert
        result.ToArray().Should().Equal(4, 5, 6, 7);
    }
}
