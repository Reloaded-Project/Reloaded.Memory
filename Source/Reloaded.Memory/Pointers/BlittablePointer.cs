using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Blittable single level pointer type that you can use with generic types such as <see cref="RefFixedArrayPtr{TStruct}"/> (to create array of pointers to structs).
    /// Requires .NET Core 3/Standard 2.1 to make good use of.
    /// Relevant issue: https://github.com/dotnet/csharplang/issues/1744 
    /// </summary>
    public unsafe struct BlittablePointer<T> where T : unmanaged
    {
        private IntPtr _value;

        /// <summary>
        /// The pointer to the value.
        /// </summary>
        public T* Pointer 
        {
            get => (T*) _value;
            set => _value = (IntPtr) value; 
        }

        /// <summary>
        /// Creates a blittable pointer
        /// </summary>
        /// <param name="pointer"></param>
        public BlittablePointer(T* pointer) => _value = (IntPtr)pointer;

        /// <summary>
        /// Converts this <see cref="BlittablePointer{T}"/> to a single level <see cref="RefPointer{TStruct}"/>.
        /// </summary>
        public RefPointer<T> AsRefPointer() => this; // Implicit Conversion

        /// <summary>
        /// Converts this <see cref="BlittablePointer{T}"/> to a value reference.
        /// </summary>
        public ref T AsReference() => ref Unsafe.AsRef<T>(Pointer); // Implicit Conversion

        /// <summary/>
        public static implicit operator BlittablePointer<T>(T* operand) => new BlittablePointer<T>(operand);
    }
}
