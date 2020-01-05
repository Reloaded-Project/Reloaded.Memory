using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Utilities
{
    public class ExtendedMemoryStream
    {
        /// <summary>
        /// Checks if the stream can write simple structs.
        /// </summary>
        [Fact]
        public void WriteStructArray()
        {
            var intStructs = new RandomIntStructGenerator(1).Structs;
            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write(intStructs);
                var bytes = extendedStream.ToArray();
                Reloaded.Memory.StructArray.FromArray<RandomIntStruct>(bytes, out var newStructs);

                Assert.Equal(intStructs, newStructs);
            };
        }

        /// <summary>
        /// Checks if the stream can write simple structs that require marshalling.
        /// </summary>
        [Fact]
        public void WriteManagedStructArray()
        {
            var marshallingStructs = new RandomMarshallingStructGenerator(1).Structs;

            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write(marshallingStructs);
                var bytes = extendedStream.ToArray();
                Reloaded.Memory.StructArray.FromArray<MarshallingStruct>(bytes, out var newStructs, true);
                Assert.Equal(marshallingStructs, newStructs);
            };
        }

        /// <summary>
        /// Checks if the stream can write a single struct that requires marshalling.
        /// </summary>
        [Fact]
        public void WriteManagedStruct()
        {
            var marshallingStruct = new MarshallingStruct
            {
                Name = "Chuck Norris", 
                CompressedSize = 420, 
                UncompressedFileSize = 942
            };

            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write(marshallingStruct);
                var bytes = extendedStream.ToArray();
                Struct.FromArray<MarshallingStruct>(bytes, out var newStruct);
                Assert.Equal(marshallingStruct, newStruct);
            };
        }

        /// <summary>
        /// Checks if the stream can write a singular struct.
        /// </summary>
        [Fact]
        public void WriteRegularStruct()
        {
            var randomIntStruct = new RandomIntStruct()
            {
                A = 1,
                B = 33,
                C = 4214
            };

            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write(randomIntStruct);
                var bytes = extendedStream.ToArray();
                Struct.FromArray<RandomIntStruct>(bytes, out var newStruct, 0);
                Assert.Equal(randomIntStruct, newStruct);
            };
        }

        /// <summary>
        /// Checks if the stream can add padding.
        /// </summary>
        [Fact]
        public void AddPadding()
        {
            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write((int) 0x0);
                extendedStream.AddPadding(2048);
                var bytes = extendedStream.ToArray();
                Assert.Equal(2048, bytes.Length);
            };
        }

        /// <summary>
        /// Checks if the stream can add padding with a custom value.
        /// </summary>
        [Fact]
        public void AddPaddingCustomValue()
        {
            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write((int)0x0);
                extendedStream.AddPadding(0x44, 2048);
                var bytes = extendedStream.ToArray();

                var slice = bytes.AsSpan().Slice(sizeof(int));
                foreach (var singleByte in slice) 
                    Assert.Equal(0x44, singleByte);
            };
        }

        /// <summary>
        /// Checks if the stream writes padding if it's aligned. It should not.
        /// </summary>
        [Fact]
        public void DoNotPadIfAligned()
        {
            using (var extendedStream = new Reloaded.Memory.Utilities.ExtendedMemoryStream())
            {
                extendedStream.Write((int) 0x0);
                extendedStream.AddPadding(sizeof(int));
                var bytes = extendedStream.ToArray();
                Assert.Equal(sizeof(int), bytes.Length);
            };
        }
    }
}
