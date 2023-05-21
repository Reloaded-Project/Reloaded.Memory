using System;
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Extensions;

public class ArrayExtensions_AsSpanFast
{
    private readonly int[] _testArray = { 1, 2, 3, 4, 5 };

    [Fact]
    public void AsSpanFast_NoParameters_ReturnsEntireSpan()
    {
        Span<int> result = _testArray.AsSpanFast();

        result.Length.Should().Be(_testArray.Length);
        for (var x = 0; x < _testArray.Length; x++)
            result[x].Should().Be(_testArray[x]);
    }

    [Fact]
    public void AsSpanFast_Offset_ReturnsSpanFromOffsetToEnd()
    {
        var offset = 2;
        Span<int> result = _testArray.AsSpanFast(offset);

        result.Length.Should().Be(_testArray.Length - offset);
        for (var x = 0; x < result.Length; x++)
            result[x].Should().Be(_testArray[offset + x]);
    }

    [Fact]
    public void AsSpanFast_OffsetAndLength_ReturnsSpanWithGivenOffsetAndLength()
    {
        var offset = 1;
        var length = 3;
        Span<int> result = _testArray.AsSpanFast(offset, length);

        result.Length.Should().Be(length);
        for (var x = 0; x < length; x++)
            result[x].Should().Be(_testArray[offset + x]);
    }

    [Fact]
    public void AsSpanFast_Range_ReturnsSpanWithinGivenRange()
    {
        Range range = 1..4;
        Span<int> result = _testArray.AsSpanFast(range);

        result.Length.Should().Be(range.End.Value - range.Start.Value);
        for (var x = 0; x < result.Length; x++)
            result[x].Should().Be(_testArray[range.Start.Value + x]);
    }

    [Fact]
    public void AsSpanFast_Index_ReturnsSpanFromIndexToEnd()
    {
        var index = new Index(2);
        Span<int> result = _testArray.AsSpanFast(index);

        result.Length.Should().Be(_testArray.Length - index.Value);
        for (var x = 0; x < result.Length; x++)
            result[x].Should().Be(_testArray[index.Value + x]);
    }
}
