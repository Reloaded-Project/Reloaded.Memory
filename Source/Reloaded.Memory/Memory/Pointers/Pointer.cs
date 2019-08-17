using System;
using System.Runtime.CompilerServices;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Represents a reference to a value of type <typeparamref name="TStruct"/>.
    /// Wraps a native pointer around a managed type, improving the ease of use.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// </summary>
    /// <typeparam name="TStruct">Value type to hold a reference to.</typeparam>
    public unsafe struct Pointer<TStruct>
    {
        /// <summary>
        /// Gets the pointer to the value.
        /// </summary>
        public void* Address { get; set; }

        /// <summary>
        /// If this is true; elements will be marshaled as they are read in and out from memory.
        /// </summary>
        public bool MarshalElements { get; set; }

        /// <summary>
        /// The source where memory will be read/written to/from.
        /// </summary>
        public IMemory Source { get; set; }

        /// <summary>
        /// Gets the value at the address where the current pointer points to.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TStruct GetValue()
        {
            Source.Read<TStruct>((IntPtr)Address, out var value, MarshalElements);
            return value;
        }

        /// <summary>
        /// Gets the value at the address where the current pointer points to.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetValue(out TStruct value)
        {
            Source.Read((IntPtr)Address, out value, MarshalElements);
        }

        /// <summary>
        /// Sets the value where the current pointer is pointing to.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(TStruct value)
        {
            Source.Write((IntPtr)Address, ref value, MarshalElements);
        }

        /// <summary>
        /// Sets the value where the current pointer is pointing to.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(ref TStruct value)
        {
            Source.Write((IntPtr)Address, ref value, MarshalElements);
        }

        /*
            ------------
            Constructors
            ------------
        */

        /// <summary>
        /// Constructs a new instance of <see cref="Pointer{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="TStruct"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        /// <param name="marshalElements">If this is true; elements will be marshaled as they are read in and out from memory.</param>
        /// <param name="memorySource">Specifies the source from which the pointer should be read/written.</param>
        public Pointer(ulong address, bool marshalElements = false, IMemory memorySource = null)
        {
            Address = (void*)address;
            MarshalElements = marshalElements;

            Source = memorySource ?? Sources.Memory.CurrentProcess;
        }
    }
}