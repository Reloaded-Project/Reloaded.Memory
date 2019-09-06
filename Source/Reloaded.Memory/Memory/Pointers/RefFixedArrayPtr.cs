using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of a set size in memory to a more familiar interface where
    /// each member of the array is returned by reference.
    /// Note: This class is not safe/does not perform range checks.
    /// It exists to provide additional functionality like LINQ which otherwise cannot be achieved without knowing amount of elements.
    /// </summary>
    public unsafe struct RefFixedArrayPtr<TStruct> where TStruct : unmanaged
    {
        /* See IArrayPtr for member definitions. */

        /// <summary>
        /// Address of the first element of the array.
        /// </summary>
        public TStruct* Pointer { get; set; }

        /// <summary>
        /// The number of elements contained in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Returns a member of this array by reference.
        /// </summary>
        public ref TStruct this[int index] => ref RefPointer<TStruct>.Create(&Pointer[index]);

        /*
            ------------
            Constructors
            ------------
        */

        /// <summary>
        /// Creates a by reference array pointer given the address of the first element of the array.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="numberOfItems">Number of items in this array.</param>
        public RefFixedArrayPtr(ulong address, int numberOfItems)
        {
            Pointer = (TStruct*) address;
            Count = numberOfItems;
        }

        /// <summary>
        /// Creates a by reference array pointer given the address of the first element of the array.
        /// </summary>
        /// <param name="structPtr">Pointer to the address of the first element of the structure array.</param>
        /// <param name="numberOfItems">Number of items in this array.</param>
        public RefFixedArrayPtr(TStruct* structPtr, int numberOfItems)
        {
            Pointer = structPtr;
            Count = numberOfItems;
        }


        /*
            --------------
            Core Functions
            --------------
        */

        /// <summary>
        /// Determines whether an element is in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TStruct item) => IndexOf(ref item) != -1;

        /// <summary>
        /// Determines whether an element is in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ref TStruct item) => IndexOf(ref item) != -1;

        /// <summary>
        /// Searches for a specified item and returns the index of the item
        /// if present.
        /// </summary>
        /// <param name="item">The item to search for in the array.</param>
        /// <returns>The index of the item, if present in the array.</returns>
        public int IndexOf(TStruct item) => IndexOf(ref item);

        /// <summary>
        /// Searches for a specified item and returns the index of the item
        /// if present.
        /// </summary>
        /// <param name="item">The item to search for in the array.</param>
        /// <returns>The index of the item, if present in the array.</returns>
        public int IndexOf(ref TStruct item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Equals(item))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Copies all the elements of the passed in sourceArray to the <see cref="RefFixedArrayPtr{TStruct}"/> array.
        /// </summary>
        /// <param name="sourceArray">The array from which to copy elements from.</param>
        /// <param name="length">The amount of elements in the source array that should be copied.</param>
        /// <param name="sourceIndex">The array index in the source array copy elements from.</param>
        /// <param name="destinationIndex">The starting index into the <see cref="FixedArrayPtr{TStruct}"/> to which elements should be copied to.</param>
        public void CopyFrom(TStruct[] sourceArray, int length, int sourceIndex = 0, int destinationIndex = 0)
        {
            int availableDestinationElements = this.Count - destinationIndex;
            if (length > availableDestinationElements)
                throw new ArgumentException($"There is insufficient space in the {nameof(RefFixedArrayPtr<TStruct>)} to copy {length} elements. (Length: {length}, Available Elements: {availableDestinationElements})");

            int availableSourceElements = sourceArray.Length - sourceIndex;
            if (length > availableSourceElements)
                throw new ArgumentException($"There is insufficient space in the {nameof(sourceArray)} to copy {length} elements. (Length: {length}, Available Elements: {availableSourceElements})");
            
            for (int x = 0; x < length; x++)
                this[destinationIndex + x] = sourceArray[sourceIndex + x];
        }

        /// <summary>
        /// Copies all the elements from the the <see cref="FixedArrayPtr{TStruct}"/> to the passed in array.
        /// </summary>
        /// <param name="destinationArray">The array from which to copy elements to.</param>
        /// <param name="length">The amount of elements in to copy to sourceArray.</param>
        /// <param name="sourceIndex">The array index in the <see cref="FixedArrayPtr{TStruct}"/> to copy elements from.</param>
        /// <param name="destinationIndex">The starting index into the <see cref="FixedArrayPtr{TStruct}"/> array to which elements should be copied to.</param>
        public void CopyTo(TStruct[] destinationArray, int length, int sourceIndex = 0, int destinationIndex = 0)
        {
            int availableDestinationElements = destinationArray.Length - destinationIndex;
            if (length > availableDestinationElements)
                throw new ArgumentException($"There is insufficient space in the destination array to copy {length} elements. (Length: {length}, Available Elements: {availableDestinationElements})");

            int availableSourceElements = this.Count - sourceIndex;
            if (length > availableSourceElements)
                throw new ArgumentException($"There are not enough elements in the current {nameof(FixedArrayPtr<TStruct>)}. (Length: {length}, Available Elements: {availableSourceElements})");

            for (int x = 0; x < length; x++)
                destinationArray[x + destinationIndex] = this[sourceIndex + x];
        }
    }
}
