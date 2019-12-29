using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of a set size in memory to a more familiar interface.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// Note: This class is not safe/does not perform range checks.
    /// It exists to provide additional functionality like LINQ which otherwise cannot be achieved without knowing amount of elements.
    /// </summary>
    public unsafe struct FixedArrayPtr<TStruct> : IEnumerable<TStruct>, IArrayPtr<TStruct>
    {
        /// <inheritdoc />
        public void* Pointer { get; set; }

        /// <inheritdoc />
        public bool MarshalElements { get; set; }

        /// <inheritdoc />
        public IMemory Source { get; set; }

        /// <inheritdoc />
        public int ElementSize => Struct.GetSize<TStruct>(MarshalElements);

        /// <summary>
        /// The number of elements contained in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// Contains the size of the entire array, in bytes.
        /// </summary>
        public int ArraySize => Struct.GetSize<TStruct>(MarshalElements) * Count;
        
        /// <summary/>
        public TStruct this[int index]
        {
            get
            {
                Get(out TStruct value, index);
                return value;
            }
            set => Set(ref value, index);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Get(out TStruct value, int index)
        {
            Source.Read((IntPtr)GetPointerToElement(index), out value, MarshalElements);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(ref TStruct value, int index)
        {
            Source.Write((IntPtr)GetPointerToElement(index), ref value, MarshalElements);
        }

        /*
            ------------
            Constructors
            ------------
        */

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <param name="marshalElements">If this is set to true elements will be marshaled as they are read in and out from memory.</param>
        /// <param name="source">Specifies the source from which the individual array elements should be read/written. This defaults to current process/local memory.</param>
        public FixedArrayPtr(ulong address, int count, bool marshalElements = false, IMemory source = null)
        {
            Pointer = (void*)address;
            Count = count;
            MarshalElements = marshalElements;

            Source = source ?? Sources.Memory.CurrentProcess;
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
        public bool Contains(TStruct item) => Contains(ref item);

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
                this.Get(out TStruct value, i);
                if (value.Equals(item))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Copies all the elements of the passed in sourceArray to the <see cref="FixedArrayPtr{TStruct}"/> array.
        /// </summary>
        /// <param name="sourceArray">The array from which to copy elements from.</param>
        /// <param name="length">The amount of elements in the source array that should be copied.</param>
        /// <param name="sourceIndex">The array index in the source array copy elements from.</param>
        /// <param name="destinationIndex">The starting index into the <see cref="FixedArrayPtr{TStruct}"/> to which elements should be copied to.</param>
        public void CopyFrom(TStruct[] sourceArray, int length, int sourceIndex = 0, int destinationIndex = 0)
        {
            // Available elements in this array.
            int availableDestinationElements = this.Count - destinationIndex;
            if (length > availableDestinationElements)
                throw new ArgumentException($"There is insufficient space in the {nameof(FixedArrayPtr<TStruct>)} to copy {length} elements. (Length: {length}, Available Elements: {availableDestinationElements})");

            int availableSourceElements = sourceArray.Length - sourceIndex;
            if (length > availableSourceElements)
                throw new ArgumentException($"There is insufficient space in the {nameof(sourceArray)} to copy {length} elements. (Length: {length}, Available Elements: {availableSourceElements})");

            // TODO: This method could be optimized if we can guarantee or make a check that the Source (IMemory) to which we are copying the memory TO is the current process.
            for (int x = 0; x < length; x++)
                this.Set(ref sourceArray[sourceIndex + x], destinationIndex + x);
        }

        /// <summary>
        /// Copies all the elements from the the <see cref="FixedArrayPtr{TStruct}"/> to the passed in sourceArray.
        /// </summary>
        /// <param name="destinationArray">The array from which to copy elements to.</param>
        /// <param name="length">The amount of elements in to copy to sourceArray.</param>
        /// <param name="sourceIndex">The array index in the <see cref="FixedArrayPtr{TStruct}"/> to copy elements from.</param>
        /// <param name="destinationIndex">The starting index into the <see cref="FixedArrayPtr{TStruct}"/> array to which elements should be copied to.</param>
        public void CopyTo(TStruct[] destinationArray, int length, int sourceIndex = 0, int destinationIndex = 0)
        {
            // Available elements in destinationArray.
            int availableDestinationElements = destinationArray.Length - destinationIndex;
            if (length > availableDestinationElements)
                throw new ArgumentException($"There is insufficient space in the {nameof(destinationArray)} to copy {length} elements. (Length: {length}, Available Elements: {availableDestinationElements})");

            int availableSourceElements = this.Count - sourceIndex;
            if (length > availableSourceElements)
                throw new ArgumentException($"There are not enough elements in the current {nameof(FixedArrayPtr<TStruct>)}. (Length: {length}, Available Elements: {availableSourceElements})");

            // Copy manually.
            for (int x = 0; x < length; x++)
            {
                this.Get(out var value, sourceIndex + x);
                destinationArray[x + destinationIndex] = value;
            }
        }

        /// <inheritdoc />
        public void* GetPointerToElement(int index)
        {
            // Do not throw, throwing exceptions makes for some very ugly code on other side.
            if (index >= Count)
                return (void*)-1;
            else
                return (void*)((long) Pointer + (index * ElementSize));
        }

        // ///////////////////////////////////////////
        // Implement IEnumerable to allow LINQ Queries
        // ///////////////////////////////////////////
        /// <inheritdoc />
        public IEnumerator<TStruct> GetEnumerator() => new FixedArrayPtrEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator()     => GetEnumerator();

        /// <summary>
        /// Implements the IEnumerator Structure for the Fixed Array Pointer, allowing for
        /// LINQ queries to be used.
        /// </summary>
        private class FixedArrayPtrEnumerator : IEnumerator<TStruct>
        {
            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            object IEnumerator.Current => Current;

            /// <summary>
            /// Contains a copy of the parent object that is to be enumerated.
            /// </summary>
            private readonly FixedArrayPtr<TStruct> _arrayPtr;

            /// <summary>
            /// Contains the index of the current element being enumerated.
            /// </summary>
            private int _currentIndex = -1;

            /// <summary>
            /// Constructor for the custom enumerator.
            /// </summary>
            /// <param name="parentArrayPtr">Contains original FixedArrayPtr this enumerator was intended for.</param>
            public FixedArrayPtrEnumerator(FixedArrayPtr<TStruct> parentArrayPtr)
            {
                _arrayPtr = parentArrayPtr;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            public TStruct Current
            {
                get
                {
                    _arrayPtr.Get(out TStruct value, _currentIndex);
                    return value;
                }
            }

            /// <summary>
            /// Advances the enumerator cursor to the next element of the collection.
            /// </summary>
            /// <returns>
            ///     True if the enumerator was successfully advanced to the next element.
            ///     False if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                // Check if we passed the end of the collection.
                _currentIndex++;

                return _currentIndex < _arrayPtr.Count;
            }

            /// <summary>
            /// Resets the current index and pointer to the defaults.
            /// </summary>
            [ExcludeFromCodeCoverage] // Technically provided for COM interoperability but never used.
            public void Reset()
            {
                _currentIndex = -1;
            }

            /// <inheritdoc />
            public void Dispose(){ }
        }
    }
}