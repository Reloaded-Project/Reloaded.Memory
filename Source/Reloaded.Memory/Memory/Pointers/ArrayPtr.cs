using System;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of unknown size in memory to a more familiar interface.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// </summary>
    public unsafe class ArrayPtr<TStruct> : IArrayPtr<TStruct>
    {
        /* See IArrayPtr for member definitions. */
        
        /// <inheritdoc />
        public void*    Pointer             { get; set; }

        /// <inheritdoc />
        public bool     MarshalElements     { get; set; }

        /// <inheritdoc />
        public IMemory  Source              { get; set; } = new Sources.Memory();

        /// <inheritdoc />
        public int      ElementSize         => Struct.GetSize<TStruct>(MarshalElements);

        /// <inheritdoc />
        public void Get(out TStruct value, int index)
        {
            Source.Read((IntPtr)GetPointerToElement(index), out value, MarshalElements);
        }

        /// <inheritdoc />
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
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element
        /// and whether elements should be marshaled or not as they are read.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="marshalElements">Set to true in order to marshal elements as they are read in and out.</param>
        /// <param name="source">Specifies the source from which the individual array elements should be read/written.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address, bool marshalElements = false, IMemory source = null)
        {
            Pointer = (void*)address;
            MarshalElements = marshalElements;

            if (source != null)
                Source = source;
        }

        /*
            --------------
            Core Functions
            --------------
        */

        /// <inheritdoc />
        public void* GetPointerToElement(int index)
        {
            return (void*)((long)Pointer + (index * ElementSize));
        }
    }
}