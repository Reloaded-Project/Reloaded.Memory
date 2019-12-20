using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Utility class which provides the ability to use a pointer of varying depth level as a by
    /// reference variable.
    /// </summary>
    public unsafe struct RefPointer<TStruct> where TStruct : unmanaged
    {
        /// <summary>
        /// The first pointer.
        /// </summary>
        public TStruct* Address { get; set; }

        /// <summary>
        /// Number of required dereferences to meet target address.
        /// </summary>
        public int DepthLevel { get; set; }

        /// <param name="address">Address of the pointer in memory.</param>
        /// <param name="depthLevel">Depth level of the pointer/number of required dereferences to meet target address. 1 = void*, 2 = void**, 3 - void*** etc.</param>
        public RefPointer(TStruct* address, int depthLevel)
        {
            Address = address;
            DepthLevel = depthLevel;
        }

        /// <summary>
        /// Attempts to dereference the pointer, returning the innermost pointer level as a ref type.
        /// If at any point along the way, the pointed to address is null, the method fails.
        /// </summary>
        public bool TryDereference(ref TStruct value)
        {
            TStruct* currentAddress = Address;

            if (currentAddress == (TStruct*) 0)
                return false;

            for (int x = 0; x < DepthLevel - 1; x++)
            {
                currentAddress = *(TStruct**)(currentAddress);
                if (currentAddress == (TStruct*) 0)
                    return false;
            }

            value = Create(currentAddress); // Lack of ref here is not a typo.
            return true;
        }

        /// <summary>
        /// Attempts to dereference the pointer, returning the innermost pointer as a pointer.
        /// If at any point along the way, the pointed to address is null, the method fails.
        /// </summary>
        public bool TryDereference(out TStruct* value)
        {
            TStruct* currentAddress = Address;
            value = (TStruct*) 0;

            if (currentAddress == (TStruct*)0)
                return false;

            for (int x = 0; x < DepthLevel - 1; x++)
            {
                currentAddress = *(TStruct**)(currentAddress);
                if (currentAddress == (TStruct*)0)
                    return false;
            }

            value = currentAddress;
            return true;
        }

        /// <summary>
        /// Attempts to dereference the pointer, returning the innermost pointer level as a ref type.
        /// If at any point along the way, the pointed to address is null, the method fails.
        /// </summary>
        /// <returns></returns>
        public ref TStruct TryDereference(out bool success)
        {
            TStruct* currentAddress = Address;
            success = false;

            if (currentAddress == (TStruct*) 0)
                return ref Create(currentAddress);

            for (int x = 0; x < DepthLevel - 1; x++)
            {
                currentAddress = *(TStruct**)(currentAddress);
                if (currentAddress == (TStruct*) 0)
                    return ref Create(currentAddress);
            }

            success = true;
            return ref Create(currentAddress);
        }

        /// <summary>
        /// Converts a pointer into a by reference variable.
        /// </summary>
        /// <param name="pointer">Pointer to the unmanaged structure.</param>
        /// <returns>A by reference variable for a given pointer.</returns>
        public static ref TStruct Create(TStruct* pointer) => ref Unsafe.AsRef<TStruct>(pointer);

        /// <summary/>
        public static implicit operator RefPointer<TStruct>(BlittablePointer<TStruct> operand) => new RefPointer<TStruct>(operand.Pointer, 1);
    }
}
