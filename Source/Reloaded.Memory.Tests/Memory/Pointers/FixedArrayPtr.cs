using System;
using System.IO;
using System.Linq;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    public class FixedArrayPtr : IDisposable
    {
        Reloaded.Memory.Sources.Memory _currentProcess;
        IntPtr _adventurePhysicsArray;
        FixedArrayPtr<AdventurePhysics> _fixedArrayPtr;

        /// <summary>
        /// The amount of elements in phys.bin. When we go test <see cref="StructArray"/>, we will test for autodetection of this value.
        /// </summary>
        public int PhysicsArrayLength = 40;

        /// <summary>
        /// Set up this function by copying over an array of Sonic Adventure physics.
        /// </summary>
        public FixedArrayPtr()
        {
            // Read in the Sonic Adventure physics array from file, then copy over to self-allocated memory.
            byte[] bytes = File.ReadAllBytes("phys.bin");

            _currentProcess = new Reloaded.Memory.Sources.Memory();
            _adventurePhysicsArray = _currentProcess.Allocate(bytes.Length);
            _currentProcess.WriteRaw(_adventurePhysicsArray, bytes);
            _fixedArrayPtr = new FixedArrayPtr<AdventurePhysics>((ulong)_adventurePhysicsArray, PhysicsArrayLength);
        }

        public void Dispose() => _currentProcess.Free(_adventurePhysicsArray);

        /// <summary>
        /// Asking for a pointer to an element out of range should return -1 from FixedArrayPtr/>.
        /// </summary>
        [Fact]
        public unsafe void GetElementPointerOutOfRange()
        {
            IArrayPtr<MarshallingStruct> fixedArrayPtr = new FixedArrayPtr<MarshallingStruct>(0, 0);
            Assert.Equal((long)(void*)-1, (long)fixedArrayPtr.GetPointerToElement(1));
        }


        /// <summary>
        /// Checks whether the Contains() method is fully functional.
        /// </summary>
        [Fact]
        public unsafe void Contains()
        {
            // Pull random elements <Count> times and verify that "Contains" returns true.
            Random randomNumberGenerator = new Random();
            for (int x = 0; x < _fixedArrayPtr.Count; x++)
            {
                int randomIndex = randomNumberGenerator.Next(0, _fixedArrayPtr.Count);
                _fixedArrayPtr.Get(out var adventurePhysics, randomIndex);
                Assert.True(_fixedArrayPtr.Contains(adventurePhysics));
            }
        }

        /// <summary>
        /// Checks whether the enumerator returns all values.
        /// </summary>
        [Fact]
        public unsafe void UseEnumerator()
        {
            int[] numbers = { 0, 2, 3, 5, 7, 8, 88, 442 };
            fixed (int* numbersPtr = numbers)
            {
                var arrayPtr = new FixedArrayPtr<int>((ulong) numbersPtr, numbers.Length);
                var numbersFromEnumerator = arrayPtr.Select(x => x).ToArray();

                Assert.Equal(numbers.Length, numbersFromEnumerator.Length);
                Assert.Equal(numbers, numbersFromEnumerator);
            }
        }

        /// <summary>
        /// Checks whether the IndexOf() method is fully functional.
        /// </summary>
        [Fact]
        public unsafe void IndexOf()
        {
            // Pull random elements <Count> times and verify that IndexOf returns correct value.
            Random randomNumberGenerator = new Random();

            // Fill in byte array with ascending numbers.
            int upperBound = 1000;
            IntPtr ptr = _currentProcess.Allocate(upperBound * sizeof(int));
            var arrayPtr = new FixedArrayPtr<int>((ulong) ptr, upperBound);

            for (int x = 0; x < upperBound; x++)
                arrayPtr.Set(ref x, x);

            // Test IndexOf
            for (int x = 0; x < arrayPtr.Count; x++)
            {
                int randomIndex = randomNumberGenerator.Next(0, arrayPtr.Count);
                arrayPtr.Get(out var value, randomIndex);
                Assert.Equal(randomIndex, arrayPtr.IndexOf(value));
            }

            // Test nonexisting item.
            int notInArray = upperBound + 1;
            Assert.Equal(-1, arrayPtr.IndexOf(notInArray));

            // Cleanup
            _currentProcess.Free(ptr);
        }

        /// <summary>
        /// Checks whether the function returns the correct internal array size.
        /// </summary>
        [Fact]
        public unsafe void ArraySize()
        {
            int expectedSize = PhysicsArrayLength * sizeof(AdventurePhysics);
            Assert.Equal(expectedSize, _fixedArrayPtr.ArraySize);
        }

        /// <summary>
        /// Tests copying from a fixed pointer array and to a fixed pointer array.
        /// </summary>
        [Fact]
        public unsafe void CopyFromCopyTo()
        {
            // Allocate array space for a copy and create a new fixed pointer to it.
            IntPtr copyPtr = _currentProcess.Allocate(_fixedArrayPtr.ArraySize);
            var arrayCopyPtr = new FixedArrayPtr<AdventurePhysics>((ulong) copyPtr, _fixedArrayPtr.Count);

            // Copy from original to new array.
            AdventurePhysics[] physicsArray = new AdventurePhysics[_fixedArrayPtr.Count];
            _fixedArrayPtr.CopyTo(physicsArray, _fixedArrayPtr.Count);
            arrayCopyPtr.CopyFrom(physicsArray, physicsArray.Length);

            // Check both copies are identical.
            for (int x = 0; x < physicsArray.Length; x++)
            {
                _fixedArrayPtr.Get(out var physicsOriginal, x);
                arrayCopyPtr.Get  (out var physicsCopied  , x);

                if (! physicsOriginal.Equals(physicsCopied))
                    Assert.True(false, "All array entries between the two fixed array pointers should be equal.");
            }

            // Cleanup
            _currentProcess.Free(copyPtr);
        }

        /// <summary>
        /// Tests copying from a fixed pointer array and to a fixed pointer array.
        /// </summary>
        [Fact]
        public unsafe void CopyFromCopyToFail()
        {
            Assert.Throws<ArgumentException>(() => _fixedArrayPtr.CopyTo(new AdventurePhysics[0], 1));      // Too big for destinationArray.
            Assert.Throws<ArgumentException>(() => _fixedArrayPtr.CopyTo(new AdventurePhysics[100], 100));  // Too big for source FixedPtrArray
            Assert.Throws<ArgumentException>(() => _fixedArrayPtr.CopyFrom(new AdventurePhysics[0], 1));    // Too big for sourceArray
            Assert.Throws<ArgumentException>(() => _fixedArrayPtr.CopyFrom(new AdventurePhysics[100], 100));// Too big for destination FixedPtrArray
        }

    }
}
