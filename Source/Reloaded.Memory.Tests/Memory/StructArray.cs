using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory
{
    public class StructArray
    {
        Reloaded.Memory.Sources.Memory _currentProcess = new Reloaded.Memory.Sources.Memory();

        /// <summary>
        /// Checks the functionality of the FromPtr() function by comparing the output of FixedArrayPtr's CopyTo and StructArray's FromPtr.
        /// </summary>
        [Fact]
        public void FromToPtr()
        {
            // Build array of random integer structs.
            int randomStructNumber = 1000;
            RandomIntStruct[] randomIntegers = new RandomIntStruct[randomStructNumber];

            for (int x = 0; x < randomStructNumber; x++)
                randomIntegers[x] = RandomIntStruct.BuildRandomStruct();

            // Allocate memory for these structs.
            IntPtr memoryPtr = _currentProcess.Allocate(Reloaded.Memory.StructArray.GetSize<RandomIntStruct>(randomStructNumber));
           
            // Write random int struct array to pointer.
            Reloaded.Memory.StructArray.ToPtr(memoryPtr, randomIntegers);

            // Read back struct array from pointer.
            Reloaded.Memory.StructArray.FromPtr<RandomIntStruct>(memoryPtr, out var randomIntegersCopy, randomStructNumber);

            // Now compare old and new array.
            Assert.Equal(randomIntegers, randomIntegersCopy);

            // Cleanup
            _currentProcess.Free(memoryPtr);
        }

        /// <summary>
        /// Tests the GetBytes() method of the StructArray function and attempts to restore original array using FromArray.
        /// </summary>
        [Fact]
        public void GetBytes()
        {
            // Build array of random integer structs.
            int randomStructNumber = 1000;
            RandomIntStruct[] randomIntegers = new RandomIntStruct[randomStructNumber];

            for (int x = 0; x < randomStructNumber; x++)
                randomIntegers[x] = RandomIntStruct.BuildRandomStruct();

            // Convert integer struct to bytes.
            byte[] bytes = Reloaded.Memory.StructArray.GetBytes(randomIntegers);

            // From bytes back to struct array.
            Reloaded.Memory.StructArray.FromArray(bytes, out RandomIntStruct[] randomIntegersCopy);

            // Compare both arrays.
            Assert.Equal(randomIntegers, randomIntegersCopy);
        }

        /// <summary>
        /// Random foreach loop test using the IEnumerator interface.
        /// </summary>
        [Fact]
        public void Foreach()
        {
            // Build array of random integer structs.
            int randomStructNumber = 1000;
            RandomIntStruct[] randomIntegers = new RandomIntStruct[randomStructNumber];

            foreach (var randomInteger in randomIntegers)
                if (! randomIntegers.Contains(randomInteger))
                    Assert.True(false, "Foreach is broken. RandomIntStruct from RandomIntStruct array not found in original array.");
        }

        /// <summary>
        /// Tests the GetSize() method of the StructArray function against manual sizeof multiplied by count.
        /// </summary>
        [Fact]
        public unsafe void GetSize()
        {
            for (int x = 0; x < 666; x++)
            {
                int arraySize = sizeof(RandomIntStruct) * x;
                Assert.Equal(arraySize, Reloaded.Memory.StructArray.GetSize<RandomIntStruct>(x));
            }
        }
    }
}
