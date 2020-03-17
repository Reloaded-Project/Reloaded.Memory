

using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Streams
{
    public partial class BufferedStreamReader
    {
		RandomInt16Generator _randomInt16Generator = new RandomInt16Generator(StructBytesMB);

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndianInt16(int bufferSize)
        {
            using (var memoryStream = _randomInt16Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt16Generator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out Int16 value);

                    var notExpected = _randomInt16Generator.Structs[x];
                    var expected    = _randomInt16Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt16Generator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianInt16(int bufferSize)
        {
            using (var memoryStream = _randomInt16Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt16Generator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out Int16 peek);
                    reader.ReadBigEndianPrimitive(out Int16 actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomInt16Generator.Structs[x];
                    var expected    = _randomInt16Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt16Generator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverloadInt16(int bufferSize)
        {
            using (var memoryStream = _randomInt16Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt16Generator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<Int16>();
                    var actual  = reader.ReadBigEndianPrimitive<Int16>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomInt16Generator.Structs[x];
                    var expected = _randomInt16Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt16Generator.Structs[x], actual);
                }
            }
        }
		RandomUInt16Generator _randomUInt16Generator = new RandomUInt16Generator(StructBytesMB);

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndianUInt16(int bufferSize)
        {
            using (var memoryStream = _randomUInt16Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt16Generator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out UInt16 value);

                    var notExpected = _randomUInt16Generator.Structs[x];
                    var expected    = _randomUInt16Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt16Generator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianUInt16(int bufferSize)
        {
            using (var memoryStream = _randomUInt16Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt16Generator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out UInt16 peek);
                    reader.ReadBigEndianPrimitive(out UInt16 actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomUInt16Generator.Structs[x];
                    var expected    = _randomUInt16Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt16Generator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverloadUInt16(int bufferSize)
        {
            using (var memoryStream = _randomUInt16Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt16Generator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<UInt16>();
                    var actual  = reader.ReadBigEndianPrimitive<UInt16>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomUInt16Generator.Structs[x];
                    var expected = _randomUInt16Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt16Generator.Structs[x], actual);
                }
            }
        }
		RandomInt32Generator _randomInt32Generator = new RandomInt32Generator(StructBytesMB);

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndianInt32(int bufferSize)
        {
            using (var memoryStream = _randomInt32Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt32Generator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out Int32 value);

                    var notExpected = _randomInt32Generator.Structs[x];
                    var expected    = _randomInt32Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt32Generator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianInt32(int bufferSize)
        {
            using (var memoryStream = _randomInt32Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt32Generator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out Int32 peek);
                    reader.ReadBigEndianPrimitive(out Int32 actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomInt32Generator.Structs[x];
                    var expected    = _randomInt32Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt32Generator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverloadInt32(int bufferSize)
        {
            using (var memoryStream = _randomInt32Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt32Generator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<Int32>();
                    var actual  = reader.ReadBigEndianPrimitive<Int32>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomInt32Generator.Structs[x];
                    var expected = _randomInt32Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt32Generator.Structs[x], actual);
                }
            }
        }
		RandomUInt32Generator _randomUInt32Generator = new RandomUInt32Generator(StructBytesMB);

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndianUInt32(int bufferSize)
        {
            using (var memoryStream = _randomUInt32Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt32Generator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out UInt32 value);

                    var notExpected = _randomUInt32Generator.Structs[x];
                    var expected    = _randomUInt32Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt32Generator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianUInt32(int bufferSize)
        {
            using (var memoryStream = _randomUInt32Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt32Generator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out UInt32 peek);
                    reader.ReadBigEndianPrimitive(out UInt32 actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomUInt32Generator.Structs[x];
                    var expected    = _randomUInt32Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt32Generator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverloadUInt32(int bufferSize)
        {
            using (var memoryStream = _randomUInt32Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt32Generator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<UInt32>();
                    var actual  = reader.ReadBigEndianPrimitive<UInt32>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomUInt32Generator.Structs[x];
                    var expected = _randomUInt32Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt32Generator.Structs[x], actual);
                }
            }
        }
		RandomInt64Generator _randomInt64Generator = new RandomInt64Generator(StructBytesMB);

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndianInt64(int bufferSize)
        {
            using (var memoryStream = _randomInt64Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt64Generator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out Int64 value);

                    var notExpected = _randomInt64Generator.Structs[x];
                    var expected    = _randomInt64Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt64Generator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianInt64(int bufferSize)
        {
            using (var memoryStream = _randomInt64Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt64Generator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out Int64 peek);
                    reader.ReadBigEndianPrimitive(out Int64 actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomInt64Generator.Structs[x];
                    var expected    = _randomInt64Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt64Generator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverloadInt64(int bufferSize)
        {
            using (var memoryStream = _randomInt64Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomInt64Generator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<Int64>();
                    var actual  = reader.ReadBigEndianPrimitive<Int64>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomInt64Generator.Structs[x];
                    var expected = _randomInt64Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomInt64Generator.Structs[x], actual);
                }
            }
        }
		RandomUInt64Generator _randomUInt64Generator = new RandomUInt64Generator(StructBytesMB);

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndianUInt64(int bufferSize)
        {
            using (var memoryStream = _randomUInt64Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt64Generator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out UInt64 value);

                    var notExpected = _randomUInt64Generator.Structs[x];
                    var expected    = _randomUInt64Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt64Generator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianUInt64(int bufferSize)
        {
            using (var memoryStream = _randomUInt64Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt64Generator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out UInt64 peek);
                    reader.ReadBigEndianPrimitive(out UInt64 actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomUInt64Generator.Structs[x];
                    var expected    = _randomUInt64Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt64Generator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverloadUInt64(int bufferSize)
        {
            using (var memoryStream = _randomUInt64Generator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomUInt64Generator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<UInt64>();
                    var actual  = reader.ReadBigEndianPrimitive<UInt64>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomUInt64Generator.Structs[x];
                    var expected = _randomUInt64Generator.Structs[x];
                    expected = Reloaded.Memory.Endian.Reverse(expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomUInt64Generator.Structs[x], actual);
                }
            }
        }
    }
}
