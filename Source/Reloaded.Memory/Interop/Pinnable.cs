using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Reloaded.Memory.Pointers;

namespace Reloaded.Memory.Interop
{
    /// <summary>
    /// Allows you to pin a native unmanaged object in a static location in memory, to be
    /// later accessible from native code.
    /// </summary>
    public unsafe class Pinnable<T> : IDisposable where T : unmanaged
    {
        /// <summary>
        /// The value pointed to by the <see cref="Pointer"/>.
        /// If the class was instantiated using an array, this is the first element of the array.
        /// </summary>
        public ref T Value => ref RefPointer<T>.Create(Pointer);

        /// <summary>
        /// Pointer to the native value in question.
        /// If the class was instantiated using an array, this is the pointer to the first element of the array.
        /// </summary>
        public T*       Pointer     { get; private set; }

        // Handle keeping the object pinned. 
        private GCHandle _handle;

        /* Constructor/Destructor */

        // Note: GCHandle.Alloc causes boxing(due to conversion to object), meaning our item is stored on the heap.
        // This means that for value types, we do not need to store them explicitly.

        /// <inheritdoc />
        public Pinnable(T[] value)
        {
            _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            Pointer = (T*) _handle.AddrOfPinnedObject();
        }

        /// <inheritdoc />
        public Pinnable(T value)
        {
            _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            Pointer = (T*) _handle.AddrOfPinnedObject();
        }

        /// <inheritdoc />
        public Pinnable(ref T value)
        {
            _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            Pointer = (T*) _handle.AddrOfPinnedObject();
        }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~Pinnable() => Dispose();

        /// <inheritdoc />
        public void Dispose()
        {
            if (_handle.IsAllocated)
                _handle.Free();

            GC.SuppressFinalize(this);
        }
    }
}
