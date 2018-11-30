using System;
using System.Runtime.CompilerServices;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of unknown size in memory to a more familiar interface.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// </summary>
    public unsafe class ArrayPtr<TStruct>
    {
        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="ArrayPtr{T}"/>.
        /// </summary>
        public void* Pointer { get; set; }

        /// <summary>
        /// If this is true; elements will be marshaled as they are read in and out from memory.
        /// </summary>
        public bool MarshalElements { get; set; }

        /// <summary>
        /// The source where memory will be read/written to/from.
        /// </summary>
        public IMemory Source { get; set; } = new Sources.Memory();

        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize => Struct.GetSize<TStruct>(MarshalElements);

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
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address)
        {
            Pointer = (void*)address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element
        /// and whether elements should be marshaled or not as they are read.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="marshalElements">Set to true in order to marshal elements as they are read in and out.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address, bool marshalElements)
        {
            Pointer = (void*)address;
            MarshalElements = marshalElements;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element
        /// and whether elements should be marshaled or not as they are read.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="marshalElements">Set to true in order to marshal elements as they are read in and out.</param>
        /// <param name="source">Specifies the source from which the individual array elements should be read/written.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address, bool marshalElements, IMemory source)
        {
            Pointer = (void*)address;
            MarshalElements = marshalElements;
            Source = source;
        }

        /*
            --------------
            Core Functions
            --------------
        */

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index">The index to retrieve a pointer for.</param>
        /// <returns>Pointer to the requested element at index.</returns>
        public void* GetPointerToElement(int index)
        {
            return (void*)((long)Pointer + (index * ElementSize));
        }
    }
}