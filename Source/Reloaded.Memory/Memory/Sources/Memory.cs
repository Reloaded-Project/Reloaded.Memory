using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using Reloaded.Memory.Exceptions;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    /// <inheritdoc />
    public unsafe class Memory : IMemory
    {
        /// <summary>
        /// Allows you to access the memory for the currently running process.
        /// </summary>
        public static Memory CurrentProcess { get; } = new Memory();

        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        /// <inheritdoc />
        public void    Read<T>(IntPtr memoryAddress, out T value, bool marshal = false)
        {
            value = marshal ? Marshal.PtrToStructure<T>(memoryAddress) : Unsafe.Read<T>((void*)memoryAddress);
        }

        /// <inheritdoc />
        public void    ReadRaw(IntPtr memoryAddress, out byte[] value, int length)
        {
            value = new byte[length];
            Marshal.Copy(memoryAddress, value, 0, value.Length);
        }

        /// <inheritdoc />
        public void    Write<T>(IntPtr memoryAddress, ref T item, bool marshal = false)
        {
            if (marshal)
                Marshal.StructureToPtr(item, memoryAddress, false);
            else
                Unsafe.Write((void*)memoryAddress, item);
        }

        /// <inheritdoc />
        public void    WriteRaw(IntPtr memoryAddress, byte[] data)
        {
            Marshal.Copy(data, 0, memoryAddress, data.Length);
        }

        /// <inheritdoc />
        public IntPtr  Allocate(int length)
        {
            // DO NOT USE Marshal.AllocHGlobal (GlobalAlloc)
            // Using AllocHGlobal will allocate our memory in a page that may be shared with other content; 
            // this may cause tests to fail when ChangePermission() is executed.

            /*
                MSDN: It is best to avoid using VirtualProtect to change page protections on memory blocks allocated by GlobalAlloc, 
                HeapAlloc, or LocalAlloc, because multiple memory blocks can exist on a single page. The heap manager assumes that all
                pages in the heap grant at least read and write access.
            */

            IntPtr returnAddress = Kernel32.VirtualAlloc
            (
                IntPtr.Zero,
                (uint)length,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
                Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );

            if (returnAddress == IntPtr.Zero)
                throw new MemoryAllocationException($"Failed to allocate memory in current process: {length} bytes, {Kernel32.GetLastError()} last error.");

            return returnAddress;
        }

        /// <inheritdoc />
        public bool    Free(IntPtr address)
        {
            Kernel32.VirtualFree(address, 0, Kernel32.MEM_ALLOCATION_TYPE.MEM_DECOMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
            return true;
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        /// <inheritdoc />
        public Kernel32.MEM_PROTECTION ChangePermission(IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions)
        {
            bool result = Kernel32.VirtualProtect(memoryAddress, (uint)size, newPermissions, out Kernel32.MEM_PROTECTION oldPermissions);

            if (!result)
                throw new MemoryPermissionException($"Unable to change permissions for the following memory address {memoryAddress.ToString("X")} of size {size} and permission {newPermissions.ToString()}");

            return oldPermissions;
        }
    }
}
