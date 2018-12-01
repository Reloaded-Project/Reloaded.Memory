using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Reloaded.Memory.Sources;
using Reloaded.Memory.Tests.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Sources
{
    /// <summary>
    /// An implementation of an IEnumerator that contains different Memory sources to test against.
    /// </summary>
    public class IMemoryGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { new Memory.Sources.Memory() },
            new object[] { new ExternalMemory(Process.GetCurrentProcess()) }
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
    }

    // ReSharper disable once InconsistentNaming
    public class IMemory
    {
        /// <summary>
        /// Attempts to allocate memory, checks if result points to valid memory (nonzero).
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void AllocateMemory(Memory.Sources.IMemory memorySource)
        {
            IntPtr pointer = memorySource.Allocate(0xFFFF);
            Assert.NotEqual((IntPtr)0, pointer);
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write primitives to a specific allocated memory address, then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWriteMemoryPrimitives(Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Random integer read/write.
            for (int x = 0; x < 100; x++)
            {
                int randomValue = new Random().Next();
                memorySource.Write(pointer, ref randomValue);
                memorySource.Read(pointer, out int randomValueCopy);
                Assert.Equal(randomValue, randomValueCopy);
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write structs to a specific allocated memory address, then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWriteMemoryStructs(Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Random struct read/write.
            for (int x = 0; x < 100; x++)
            {
                RandomIntStruct randomIntStruct = RandomIntStruct.BuildRandomStruct();
                memorySource.Write(pointer, ref randomIntStruct);
                memorySource.Read(pointer, out RandomIntStruct randomValueCopy);
                Assert.Equal(randomIntStruct, randomValueCopy);
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }


        /// <summary>
        /// Attempts to write structs, WITH MARSHALLING to a specific allocated memory address. Then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWriteMemoryMarshallingStructs(Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Random marshal struct read/write.
            for (int x = 0; x < 100; x++)
            {
                MarshallingStruct randomIntStruct = MarshallingStruct.BuildRandomStruct();
                memorySource.Write(pointer, ref randomIntStruct, true);
                memorySource.Read(pointer, out MarshallingStruct randomValueCopy, true);

                // Test for equality.
                Assert.Equal(randomIntStruct, randomValueCopy);

                // Test references:
                // If marshalling did not take place, write function would have written pointer to string and read it back in.
                // If marshalling did take place, a new string was created with the value of the string found in memory.
                // Set marshal parameter to false in read/write operation above to test this.
                Assert.False(object.ReferenceEquals(randomIntStruct.Name, randomValueCopy.Name));
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }

    }
}
