using System;
using System.IO;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Streams
{
    public partial class BufferedStreamReader
    {
        /* Target size of buffers for testing. */
        public const int AlignedBufferSize = 128;
        public const int MisalignedBufferSize = 129;

        /* Amount of randomized data in megabytes. */
        public const int StructBytesMB = 1;

        /* Test Data */
        private RandomIntStructGenerator _randomIntStructGenerator;
        private RandomInt32Generator _randomIntegerGenerator;

        /* Construction/Destruction */
        public BufferedStreamReader()
        {
            _randomIntStructGenerator = new RandomIntStructGenerator(StructBytesMB);
            _randomIntegerGenerator = new RandomInt32Generator(StructBytesMB);
        }

        /* Read back all structs and compare. */
        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElements(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.Read(out RandomIntStruct value);
                    Assert.Equal(_randomIntStructGenerator.Structs[x], value);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    reader.Read(out int value);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElements(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.Peek(out RandomIntStruct expected);
                    reader.Read(out RandomIntStruct actual);
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntStructGenerator.Structs[x], actual);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    reader.Peek(out int expected);
                    reader.Read(out int actual);
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsOverload(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    var expected = reader.Peek<RandomIntStruct>();
                    var actual   = reader.Read<RandomIntStruct>();
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntStructGenerator.Structs[x], actual);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    var expected    = reader.Peek<int>();
                    var actual      = reader.Read<int>();
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], actual);
                }
            }
        }

        /* Read back all structs and compare. */
        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsBigEndian(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.ReadBigEndianStruct(out RandomIntStruct value);

                    var notExpected = _randomIntStructGenerator.Structs[x];
                    var expected = _randomIntStructGenerator.Structs[x];
                    expected.SwapEndian();

                    Assert.Equal(expected, value);
                    if (! expected.Equals(notExpected))
                        Assert.NotEqual(notExpected, value);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    reader.ReadBigEndianPrimitive(out int value);

                    var notExpected = _randomIntegerGenerator.Structs[x];
                    var expected    = _randomIntegerGenerator.Structs[x];
                    Reloaded.Memory.Endian.Reverse(ref expected);

                    Assert.Equal(expected, value);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomIntegerGenerator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndian(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.PeekBigEndianStruct(out RandomIntStruct peek);
                    reader.ReadBigEndianStruct(out RandomIntStruct actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomIntStructGenerator.Structs[x];
                    var expected    = _randomIntStructGenerator.Structs[x];
                    expected.SwapEndian();

                    Assert.Equal(expected, actual);
                    if (!expected.Equals(notExpected))
                        Assert.NotEqual(notExpected, actual);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    reader.PeekBigEndianPrimitive(out int peek);
                    reader.ReadBigEndianPrimitive(out int actual);
                    Assert.Equal(peek, actual);

                    var notExpected = _randomIntegerGenerator.Structs[x];
                    var expected    = _randomIntegerGenerator.Structs[x];
                    Reloaded.Memory.Endian.Reverse(ref expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomIntegerGenerator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsBigEndianOverload(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianStruct<RandomIntStruct>();
                    var actual  = reader.ReadBigEndianStruct<RandomIntStruct>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomIntStructGenerator.Structs[x];
                    var expected = _randomIntStructGenerator.Structs[x];
                    expected.SwapEndian();

                    Assert.Equal(expected, actual);
                    if (!expected.Equals(notExpected))
                        Assert.NotEqual(notExpected, actual);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    var peek    = reader.PeekBigEndianPrimitive<int>();
                    var actual  = reader.ReadBigEndianPrimitive<int>();
                    Assert.Equal(peek, actual);

                    var notExpected = _randomIntegerGenerator.Structs[x];
                    var expected = _randomIntegerGenerator.Structs[x];
                    Reloaded.Memory.Endian.Reverse(ref expected);

                    Assert.Equal(expected, actual);
                    if (expected != notExpected)
                        Assert.NotEqual(_randomIntegerGenerator.Structs[x], actual);
                }
            }
        }

        /* Read back all structs and compare. */
        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void ReadAllElementsManaged(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.Read(out RandomIntStruct value, true);
                    Assert.Equal(_randomIntStructGenerator.Structs[x], value);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    reader.Read(out int value, true);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsManaged(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.Peek(out RandomIntStruct expected, true);
                    reader.Read(out RandomIntStruct actual, true);
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntStructGenerator.Structs[x], actual);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    reader.Read(out int value, true);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], value);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void PeekAllElementsManagedOverload(int bufferSize)
        {
            // Int Structs: Complex
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    var expected    = reader.Peek<RandomIntStruct>(true);
                    var actual      = reader.Read<RandomIntStruct>(true);
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntStructGenerator.Structs[x], actual);
                }
            }

            // Integers: Primitive
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);
                for (int x = 0; x < _randomIntegerGenerator.Structs.Length; x++)
                {
                    var expected = reader.Peek<int>(true);
                    var actual   = reader.Read<int>(true);
                    Assert.Equal(expected, actual);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], actual);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void SeekOutsideBuffer(int bufferSize)
        {
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);

                // Structs skipped each seek and amount of bytes seeked.
                int sizeOfStruct = Struct.GetSize<RandomIntStruct>();
                int structsSkip = (bufferSize * 2) / sizeOfStruct;
                int bytesSeek = structsSkip * sizeOfStruct;

                int currentStructIndex = 0;

                // Seek to said index and compare, multiple times.
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length / structsSkip; x++)
                {
                    // Read current
                    reader.Read(out RandomIntStruct value);
                    Assert.Equal(_randomIntStructGenerator.Structs[currentStructIndex], value);

                    // Skip structsSkip amount of structs. Remove sizeOfStruct from bytesSeek as reading once
                    // auto-advanced internal pointer by 1 struct.
                    currentStructIndex += structsSkip;
                    reader.Seek(bytesSeek - sizeOfStruct, SeekOrigin.Current);
                }
            }
        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void SeekInsideAndOutsideBuffer(int bufferSize)
        {
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);

                // Structs skipped each seek and amount of bytes seeked.
                int sizeOfStruct = Struct.GetSize<RandomIntStruct>();
                int structsSkip = (bufferSize / 4) / sizeOfStruct;
                int bytesSeek = structsSkip * sizeOfStruct;

                int currentStructIndex = 0;

                // Seek to said index and compare, multiple times.
                for (int x = 0; x < _randomIntStructGenerator.Structs.Length / structsSkip; x++)
                {
                    // Read current
                    reader.Read(out RandomIntStruct value);
                    Assert.Equal(_randomIntStructGenerator.Structs[currentStructIndex], value);

                    // Skip structsSkip amount of structs. Remove sizeOfStruct from bytesSeek as reading once
                    // auto-advanced internal pointer by 1 struct.
                    currentStructIndex += structsSkip;
                    reader.Seek(bytesSeek - sizeOfStruct, SeekOrigin.Current);
                }
            }

        }

        [Theory]
        [InlineData(AlignedBufferSize)]
        [InlineData(MisalignedBufferSize)]
        public void SeekBeginAndEndOutsideBuffer(int bufferSize)
        {
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {   
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, bufferSize);

                // Structs skipped each seek and amount of bytes seeked.
                int sizeOfStruct = Struct.GetSize<RandomIntStruct>();
                int structsSkip = (bufferSize * 2) / sizeOfStruct;
                int bytesSeek = structsSkip * sizeOfStruct;

                // Beginning 
                reader.Seek(bytesSeek, SeekOrigin.Begin);
                reader.Read(out RandomIntStruct beginValue);
                Assert.Equal(_randomIntStructGenerator.Structs[structsSkip], beginValue);

                // End
                reader.Seek(bytesSeek, SeekOrigin.End);
                reader.Read(out RandomIntStruct endValue);
                Assert.Equal(_randomIntStructGenerator.Structs[_randomIntStructGenerator.Structs.Length - structsSkip], endValue);
            }
        }

        [Fact]
        public void SeekBackwards()
        {
            using (var memoryStream = _randomIntStructGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, Struct.GetSize<RandomIntStruct>() * 4);

                // Structs skipped each seek and amount of bytes seeked.
                int sizeOfStruct = Struct.GetSize<RandomIntStruct>();

                for (int x = 0; x < _randomIntStructGenerator.Structs.Length; x++)
                {
                    reader.Read(out RandomIntStruct firstRead);
                    reader.Seek(-sizeOfStruct, SeekOrigin.Current);
                    reader.Read(out RandomIntStruct secondRead);
                    Assert.Equal(firstRead, secondRead);
                }
            }
        }

        [Fact]
        public void ReadBytes()
        {
            // Subarray utility class.
            T[] SubArray<T>(T[] data, int index, int length)
            {
                T[] result = new T[length];
                Array.Copy(data, index, result, 0, length);
                return result;
            }

            // Test which alternates between reading middle of bytes and sequential array entries.
            using (var memoryStream = _randomIntegerGenerator.GetMemoryStream())
            {
                var reader = new Reloaded.Memory.Streams.BufferedStreamReader(memoryStream, 4096);
                int halfwayPoint = _randomIntegerGenerator.Bytes.Length / 2;
                int byteCount = 200;
                int iterations = 5;

                for (int x = 0; x < iterations; x++)
                {
                    // Read real.
                    reader.Read(out int value, true);
                    Assert.Equal(_randomIntegerGenerator.Structs[x], value);

                    // Read middle of array.
                    byte[] actual = reader.ReadBytes(halfwayPoint, byteCount);
                    byte[] expected = SubArray(_randomIntegerGenerator.Bytes, halfwayPoint, byteCount);
                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
