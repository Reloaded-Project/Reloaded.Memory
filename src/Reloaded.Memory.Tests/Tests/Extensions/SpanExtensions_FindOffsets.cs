using System;
using System.Collections.Generic;
using FluentAssertions;
using Reloaded.Memory.Extensions;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Extensions;

/// <summary>
///     Tests for finding offsets.
/// </summary>
public class SpanExtensions_FindOffsets
{
    public enum FindOffsetMethod
    {
        Avx2,
        Sse2,
        Fallback
    }

    /// <summary>
    ///     Determines if offsets for StringPool can be correctly found using various optimized approaches.
    /// </summary>
    [Theory]
#if NETCOREAPP3_1_OR_GREATER
    [InlineData(65, new[] { 1, 4, 8 }, FindOffsetMethod.Avx2)] // above register
    [InlineData(64, new[] { 1, 4, 8 }, FindOffsetMethod.Avx2)] // on register
    [InlineData(64, new[] { 63 }, FindOffsetMethod.Avx2)] // last element
    [InlineData(64, new[] { 0, 63 }, FindOffsetMethod.Avx2)] // first element
    [InlineData(63, new[] { 1, 4, 8 }, FindOffsetMethod.Avx2)] // below register
    [InlineData(33, new[] { 1, 4, 8 }, FindOffsetMethod.Sse2)] // above register
    [InlineData(32, new[] { 1, 4, 8 }, FindOffsetMethod.Sse2)] // on register
    [InlineData(32, new[] { 31 }, FindOffsetMethod.Sse2)] // last element
    [InlineData(32, new[] { 0, 31 }, FindOffsetMethod.Sse2)] // first element
    [InlineData(31, new[] { 1, 4, 8 }, FindOffsetMethod.Sse2)] // below register
#endif
    [InlineData(8, new[] { 1, 4, 7 }, FindOffsetMethod.Fallback)] // above register
    [InlineData(8, new[] { 7 }, FindOffsetMethod.Fallback)] // last element
    [InlineData(8, new[] { 0, 7 }, FindOffsetMethod.Fallback)] // first element
    public void FindOffset(int numBytes, int[] expectedOffsets, FindOffsetMethod findOffsetMethod)
    {
        var bytes = GenerateRandomBytes(numBytes, expectedOffsets);
        List<int> offsets = FindOffsetWithMethod(bytes, findOffsetMethod);

        offsets.Count.Should().Be(expectedOffsets.Length);
        for (var x = 0; x < expectedOffsets.Length; x++)
            offsets[x].Should().Be(expectedOffsets[x]);
    }

    private unsafe List<int> FindOffsetWithMethod(byte[] data, FindOffsetMethod method)
    {
        fixed (byte* bytePtr = data)
        {
            var results = new List<int>();
            switch (method)
            {
#if NETCOREAPP3_1_OR_GREATER
                case FindOffsetMethod.Avx2:
                    SpanExtensions.FindAllOffsetsOfByteAvx2(bytePtr, data.Length, 0, results);
                    break;
                case FindOffsetMethod.Sse2:
                    SpanExtensions.FindAllOffsetsOfByteSse2(bytePtr, data.Length, 0, results);
                    break;
#endif
                case FindOffsetMethod.Fallback:
                    SpanExtensions.FindAllOffsetsOfByteFallback(bytePtr, data.Length, 0, 0, results);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }

            return results;
        }
    }

    private byte[] GenerateRandomBytes(int numBytes, int[] offsets)
    {
        var result = new byte[numBytes];
        for (var x = 0; x < result.Length; x++)
            result[x] = (byte)(x == 0 ? 1 : x);

        for (var x = 0; x < offsets.Length; x++)
            result[offsets[x]] = 0;

        return result;
    }
}
