using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of unknown size in memory to a more familiar interface where
    /// each member of the array is returned by reference.
    /// </summary>
    /// <typeparam name="TStruct">A unmanaged native structure in the memory of the current process.</typeparam>
    public unsafe struct RefArrayPtr<TStruct> where TStruct : unmanaged
    {
        /* See IArrayPtr for member definitions. */

        /// <summary>
        /// Address of the first element of the array.
        /// </summary>
        public TStruct* Pointer { get; private set; }

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
        public RefArrayPtr(ulong address) => Pointer = (TStruct*) address;

        /// <summary>
        /// Creates a by reference array pointer given the address of the first element of the array.
        /// </summary>
        /// <param name="structPtr">Pointer to the address of the first element of the structure array.</param>
        public RefArrayPtr(TStruct* structPtr) => Pointer = (TStruct*)structPtr;
    }
}
