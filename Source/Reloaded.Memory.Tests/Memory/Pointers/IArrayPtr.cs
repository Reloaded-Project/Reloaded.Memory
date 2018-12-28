using System;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    /// <summary>
    /// Tests the classes which implement from <see cref="IArrayPtr{TStruct}"/>.
    /// </summary>
    public class IArrayPtr
    {
        /// <summary>
        /// Tests for getting the element size of an individual struct element,
        /// OF A STRUCT WHOSE SIZE CHANGES AFTER MARSHALLING.
        /// </summary>
        [Fact]
        public void MarshalledArrayElementSize()
        {
            int marshalledSizeOfStruct = 8 + 16; // 8  = 2 ints
            // 16 = Inline Array of ANSI Characters

            int nativeSizeOfStruct = 8 + IntPtr.Size;
            // Same as above, except treating string as pointer.

            // Actual elements not needed.
            IArrayPtr<MarshallingStruct> arrayPtr = new ArrayPtr<MarshallingStruct>(0);
            IArrayPtr<MarshallingStruct> fixedArrayPtr = new FixedArrayPtr<MarshallingStruct>(0, 0);

            arrayPtr.MarshalElements = true;
            fixedArrayPtr.MarshalElements = true;
            Assert.Equal(marshalledSizeOfStruct, arrayPtr.ElementSize);
            Assert.Equal(marshalledSizeOfStruct, fixedArrayPtr.ElementSize);

            arrayPtr.MarshalElements = false;
            fixedArrayPtr.MarshalElements = false;
            Assert.Equal(nativeSizeOfStruct, arrayPtr.ElementSize);
            Assert.Equal(nativeSizeOfStruct, fixedArrayPtr.ElementSize);
        }

        /// <summary>
        /// Tests getting the element size of an individual struct element.
        /// </summary>
        /// <param name="adventurePhysicsPointer">A pointer to an array of Sonic Adventure physics in arbitrary memory.</param>
        [Theory]
        [ClassData(typeof(IArrayPtrGenerator))]
        public void ArrayElementSize(IArrayPtr<AdventurePhysics> adventurePhysicsPointer)
        {
            // No change in element size, this class does not do marshalling.
            adventurePhysicsPointer.MarshalElements = true;
            Assert.Equal(0x84, adventurePhysicsPointer.ElementSize);

            adventurePhysicsPointer.MarshalElements = false;
            Assert.Equal(0x84, adventurePhysicsPointer.ElementSize);
        }

        /// <summary>
        /// Tests getting and setting of the pointer by changing the pointer to the next element and comparing elements.
        /// </summary>
        /// <param name="adventurePhysicsPointer">A pointer to an array of Sonic Adventure physics in arbitrary memory.</param>
        [Theory]
        [ClassData(typeof(IArrayPtrGenerator))]
        public unsafe void GetSetPointer(IArrayPtr<AdventurePhysics> adventurePhysicsPointer)
        {
            // No change in element size, this class does not do marshalling.
            
            // Backup original pointer.
            void* originalPtr = adventurePhysicsPointer.Pointer;
            int elementSize   = adventurePhysicsPointer.ElementSize;

            // Get first two elements.
            adventurePhysicsPointer.Get(out var firstElement , 0);
            adventurePhysicsPointer.Get(out var secondElement, 1);

            // Check Indexer overloads.
            Assert.Equal(firstElement, adventurePhysicsPointer[0]);
            Assert.Equal(secondElement, adventurePhysicsPointer[1]);

            Assert.NotEqual(firstElement, secondElement);           // First two elements should not be equal. If they are, change your test data.

            // Increment pointer by 1 and get first element (true second element).
            adventurePhysicsPointer.Pointer = (void*) IntPtr.Add((IntPtr)originalPtr, elementSize);

            // Get first (second) element and compare.
            adventurePhysicsPointer.Get(out var pseudoFirstElement, 0);

            Assert.NotEqual(firstElement, pseudoFirstElement);
            Assert.Equal   (secondElement, pseudoFirstElement);

            // Restore pointer.
            adventurePhysicsPointer.Pointer = originalPtr;
        }

        /// <summary>
        /// Tests changing an individual element in an <see cref="IArrayPtr{TStruct}"/>.
        /// </summary>
        /// <param name="adventurePhysicsPointer">A pointer to an array of Sonic Adventure physics in arbitrary memory.</param>
        [Theory]
        [ClassData(typeof(IArrayPtrGenerator))]
        public void SetArrayIndex(IArrayPtr<AdventurePhysics> adventurePhysicsPointer)
        {
            // Grab First and Second Element
            adventurePhysicsPointer.Get(out var firstElement, 0);
            adventurePhysicsPointer.Get(out var secondElement, 1);

            // Check Indexer overloads.
            Assert.Equal(firstElement, adventurePhysicsPointer[0]);
            Assert.Equal(secondElement, adventurePhysicsPointer[1]);

            // Confirm not equal.
            Assert.NotEqual(firstElement, secondElement);           // First two elements should not be equal. If they are, change your test data.

            // Set second element, get copy of it.
            adventurePhysicsPointer.Set(ref firstElement, 1);
            adventurePhysicsPointer.Get(out var newSecondElement, 1);

            Assert.NotEqual(secondElement, newSecondElement);
            Assert.Equal   (firstElement , newSecondElement);

            // Set original element with Index setter.
            adventurePhysicsPointer[1] = secondElement;
            var restoredSecondelement = adventurePhysicsPointer[1];

            Assert.Equal(secondElement, restoredSecondelement);
            Assert.NotEqual(secondElement, firstElement);

            // Restore
            adventurePhysicsPointer.Set(ref secondElement, 1);
        }



    }
}
