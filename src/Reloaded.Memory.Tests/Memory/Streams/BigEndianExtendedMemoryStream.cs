


using System;
using System.IO;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Tests.Memory.Helpers;
using Reloaded.Memory.Streams.Writers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Streams
{
    public partial class ExtendedMemoryStream
    {
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianInt16Array()
        {
            var integers = new RandomInt16Generator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<Int16>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianUInt16Array()
        {
            var integers = new RandomUInt16Generator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<UInt16>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianInt32Array()
        {
            var integers = new RandomInt32Generator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<Int32>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianUInt32Array()
        {
            var integers = new RandomUInt32Generator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<UInt32>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianInt64Array()
        {
            var integers = new RandomInt64Generator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<Int64>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianUInt64Array()
        {
            var integers = new RandomUInt64Generator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<UInt64>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianSingleArray()
        {
            var integers = new RandomSingleGenerator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<Single>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }
        /// <summary>
        /// Checks if the stream can write simple Big Endian primitives.
        /// </summary>
        [Fact]
        public void WriteBigEndianDoubleArray()
        {
            var integers = new RandomDoubleGenerator(1).Structs;
            using (var extendedStream = new BigEndianMemoryStream(new Reloaded.Memory.Streams.ExtendedMemoryStream()))
            {
                extendedStream.Write(integers);
                Reloaded.Memory.StructArray.FromArrayBigEndianPrimitive<Double>(extendedStream.ToArray(), out var newStructs);

                Assert.Equal(integers, newStructs);
            };
        }

    }
}