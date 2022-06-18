using System;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Reloaded.Memory.Exceptions;

namespace Reloaded.Memory.Sources
{
    /// <inheritdoc />
    public unsafe class Memory : IMemory
    {
        /// <summary>
        /// Allows you to access the memory for the currently running process.
        /// (Alias for <see cref="Instance"/>)
        /// </summary>
        public static Memory CurrentProcess => Instance;

        /// <summary>
        /// Allows you to access the memory for the currently running process.
        /// </summary>
        public static Memory Instance { get; } = new Memory();

        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        /// <inheritdoc />
        public void Read<T>(nuint memoryAddress, out T value) where T : unmanaged
        {
            value = Unsafe.Read<T>((void*)memoryAddress);
        }

        /// <inheritdoc />
        public void Read<
#if NET5_0_OR_GREATER 
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(nuint memoryAddress, out T value, bool marshal)
        {
            value = marshal ? Marshal.PtrToStructure<T>(unchecked((nint)memoryAddress)) : Unsafe.Read<T>((void*)memoryAddress);
        }

        /// <inheritdoc />
        public void ReadRaw(nuint memoryAddress, out byte[] value, int length)
        {
#if NET5_0_OR_GREATER
            value = GC.AllocateUninitializedArray<byte>(length, false);
#else
            value = new byte[length];
#endif
            Marshal.Copy(unchecked((nint)memoryAddress), value, 0, value.Length);
        }

        /// <inheritdoc />
        public void Write<T>(nuint memoryAddress, ref T item) where T : unmanaged
        {
            Unsafe.Write((void*)memoryAddress, item);
        }

        /// <inheritdoc />
        public void Write<T>(nuint memoryAddress, ref T item, bool marshal)
        {
            if (marshal)
                Marshal.StructureToPtr(item, unchecked((nint)memoryAddress), false);
            else
                Unsafe.Write((void*)memoryAddress, item);
        }

        /// <inheritdoc />
        public void WriteRaw(nuint memoryAddress, byte[] data)
        {
            Marshal.Copy(data, 0, unchecked((nint)memoryAddress), data.Length);
        }

        /// <inheritdoc />
        public nuint Allocate(int length)
        {
            // DO NOT USE Marshal.AllocHGlobal (GlobalAlloc)
            // Using AllocHGlobal will allocate our memory in a page that may be shared with other content; 
            // this may cause tests to fail when ChangePermission() is executed.

            /*
                MSDN: It is best to avoid using VirtualProtect to change page protections on memory blocks allocated by GlobalAlloc, 
                HeapAlloc, or LocalAlloc, because multiple memory blocks can exist on a single page. The heap manager assumes that all
                pages in the heap grant at least read and write access.
            */

            nuint returnAddress = Kernel32.Kernel32.VirtualAlloc
            (
                UIntPtr.Zero,
                (UIntPtr) length,
                Kernel32.Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
                Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );

            if (returnAddress == 0)
                throw new MemoryAllocationException($"Failed to allocate memory in current process: {length} bytes, {Marshal.GetLastWin32Error()} last error.");

            return returnAddress;
        }

        /// <inheritdoc />
        public bool    Free(nuint address)
        {
            Kernel32.Kernel32.VirtualFree(address, (UIntPtr) 0, Kernel32.Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
            return true;
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        /// <inheritdoc />
        public Kernel32.Kernel32.MEM_PROTECTION ChangePermission(nuint memoryAddress, int size, Kernel32.Kernel32.MEM_PROTECTION newPermissions)
        {
            bool result = Kernel32.Kernel32.VirtualProtect(memoryAddress, (UIntPtr) size, newPermissions, out Kernel32.Kernel32.MEM_PROTECTION oldPermissions);

            if (!result)
                throw new MemoryPermissionException($"Unable to change permissions for the following memory address {memoryAddress} of size {size} and permission {newPermissions}");

            return oldPermissions;
        }
    }
}
