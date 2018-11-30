using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of a set size in memory to a more familiar interface.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// </summary>
    public unsafe class FixedArrayPtr<TStruct> : IEnumerable<TStruct>
    {
        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        public void* Pointer { get; set; }

        /// <summary>
        /// The number of elements contained in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// If this is true; elements will be marshaled as they are read in and out from memory.
        /// </summary>
        public bool MarshalElements { get; set; }

        /// <summary>
        /// The source where memory will be read/written.
        /// </summary>
        public IMemory Source { get; set; } = new Sources.Memory();

        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize => Struct.GetSize<TStruct>(MarshalElements);

        /// <summary>
        /// Contains the size of the entire array, in bytes.
        /// </summary>
        public int ArraySize => Struct.GetSize<TStruct>(MarshalElements) * Count;

        /// <summary>
        /// Gets the value of an item at a specific index.
        /// </summary>
        /// <param name="value">The value to be received from the array.</param>
        /// <param name="index">The index in the array from which to receive the value.</param>
        public void GetValue(out TStruct value, int index)
        {
            Source.Read((IntPtr)GetPointerToElement(index), out value, MarshalElements);
        }

        /// <summary>
        /// Sets the value of an item at a specific index.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        /// <param name="index">The index in the array to which the value is to be written to.</param>
        public void SetValue(ref TStruct value, int index)
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
        public FixedArrayPtr(ulong address, int count)
        {
            Pointer = (void*)address;
            Count = count;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <param name="marshalElements">If this is set to true elements will be marshaled as they are read in and out from memory.</param>
        public FixedArrayPtr(ulong address, int count, bool marshalElements)
        {
            Pointer = (void*)address;
            Count = count;
            MarshalElements = marshalElements;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <param name="source">Specifies the source from which the individual array elements should be read/written.</param>
        public FixedArrayPtr(ulong address, int count, IMemory source)
        {
            Pointer = (void*)address;
            Count = count;
            Source = source;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <param name="marshalElements">If this is set to true elements will be marshaled as they are read in and out from memory.</param>
        /// <param name="source">Specifies the source from which the individual array elements should be read/written.</param>
        public FixedArrayPtr(ulong address, int count, bool marshalElements, IMemory source)
        {
            Pointer = (void*)address;
            Count = count;
            MarshalElements = marshalElements;
            Source = source;
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
        public bool Contains(TStruct item) => IndexOf(item) != -1;

        /// <summary>
        /// Searches for a specified item and returns the index of the item
        /// if present.
        /// </summary>
        /// <param name="item">The item to search for in the array.</param>
        /// <returns>The index of the item, if present in the array.</returns>
        public int IndexOf(TStruct item)
        {
            for (int i = 0; i < Count; i++)
            {
                this.GetValue(out TStruct value, i);
                if (value.Equals(item))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Copies all the elements of the passed in array to the <see cref="FixedArrayPtr{TStruct}"/> array.
        /// </summary>
        /// <param name="sourceArray">The array from which to copy elements from.</param>
        /// <param name="sourceArrayIndex">The array index in the source array copy elements from.</param>
        /// <param name="sourceArrayCount">The amount of elements in the source array that should be copied.</param>
        /// <param name="destinationIndex">The starting index into the <see cref="FixedArrayPtr{TStruct}"/> array to which elements should be copied to.</param>
        public void CopyTo(TStruct[] sourceArray, int sourceArrayCount, int sourceArrayIndex = 0, int destinationIndex = 0)
        {
            // Basic exception handling.
            int availableElements = Count - destinationIndex;
            if (sourceArrayCount > availableElements)
                throw new ArgumentException($"There is insufficient space in the current array to copy {sourceArrayCount} elements. ({sourceArrayCount} > {availableElements})");

            // TODO: This method could be optimized if we can guarantee or make a check that the Source (IMemory) to which we are copying the memory TO is the current process.
            for (int x = 0; x < sourceArrayCount; x++)
            {
                int thisArrayIndex = destinationIndex + x;
                this.SetValue(ref sourceArray[sourceArrayIndex + x], thisArrayIndex);
            }
        }

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index">The index to retrieve a pointer for.</param>
        /// <returns>
        ///     Pointer to the requested element at index.
        ///     -1 if the element is not part of the collection.
        /// </returns>
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
            public TStruct Current
            {
                get
                {
                    _arrayPtr.GetValue(out TStruct value, _currentIndex);
                    return value;
                }
            }

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
            private int _currentIndex = 0;

            /// <summary>
            /// Constructor for the custom enumerator.
            /// </summary>
            /// <param name="parentArrayPtr">Contains original FixedArrayPtr this enumerator was intended for.</param>
            public FixedArrayPtrEnumerator(FixedArrayPtr<TStruct> parentArrayPtr)
            {
                _arrayPtr = parentArrayPtr;
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
            public void Reset()
            {
                _currentIndex = 0;
            }

            /// <inheritdoc />
            public void Dispose(){ }
        }
    }
}