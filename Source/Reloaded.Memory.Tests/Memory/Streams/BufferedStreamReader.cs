using System;
using System.IO;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Streams
{
    public class BufferedStreamReader
    {
        /* Target size of buffers for testing. */
        public const int AlignedBufferSize = 128;
        public const int MisalignedBufferSize = 129;

        /* Amount of randomized data in megabytes. */
        public const int StructBytesMB = 10;

        /* Test Data */
        private RandomIntStructGenerator _randomIntStructGenerator;
        private RandomIntegerGenerator _randomIntegerGenerator;

        /* Construction/Destruction */
        public BufferedStreamReader()
        {
            _randomIntStructGenerator = new RandomIntStructGenerator(StructBytesMB);
            _randomIntegerGenerator = new RandomIntegerGenerator(StructBytesMB);
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
