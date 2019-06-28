using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Reloaded.Memory.Exceptions;

namespace Reloaded.Memory.Sources
{
    /// <summary>
    /// Provides access to memory of another process on a Windows machine.
    /// </summary>
    public unsafe class ExternalMemory : IMemory
    {
        /// <summary>
        /// Contains the current process' memory.
        /// </summary>
        private static Memory _localMemory = new Memory();

        /// <summary>
        /// Contains the handle of the process used to read memory
        /// from and write memory to external process.
        /// </summary>
        private readonly IntPtr _processHandle;
        
        /*
             --------------
             Constructor(s)
             --------------
        */

        /// <summary>
        /// Creates an instance of the <see cref="ExternalMemory"/> class used to read from an
        /// external process with a specified handle.
        /// </summary>
        /// <param name="processHandle">Handle of the process to read/write memory from.</param>
        public ExternalMemory(IntPtr processHandle)
        {
            _processHandle  = processHandle;
        }

        /// <summary>
        /// Creates an instance of the <see cref="ExternalMemory"/> class used to read from an
        /// external process with a specified handle.
        /// </summary>
        /// <param name="process">The individual process to read/write memory from.</param>
        public ExternalMemory(Process process) : this (process.Handle) { }

        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        /// <inheritdoc />
        public void Read<T>(IntPtr memoryAddress, out T value) where T : unmanaged
        {
            int structSize = Struct.GetSize<T>();
            byte[] buffer = new byte[structSize];

            fixed (byte* bufferPtr = buffer)
            {
                bool succeeded = Kernel32.Kernel32.ReadProcessMemory(_processHandle, memoryAddress, (IntPtr)bufferPtr, (UIntPtr)structSize, out _);
                if (!succeeded)
                    throw new MemoryException($"ReadProcessMemory failed to read {structSize} bytes of memory from {memoryAddress.ToString("X")}");

                _localMemory.Read((IntPtr)bufferPtr, out value);
            }
        }

        /// <inheritdoc />
        public void Read<T>(IntPtr memoryAddress, out T value, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);
            byte[] buffer  = new byte[structSize];

            fixed (byte* bufferPtr = buffer)
            {
                bool succeeded = Kernel32.Kernel32.ReadProcessMemory(_processHandle, memoryAddress, (IntPtr)bufferPtr, (UIntPtr) structSize, out _);
                if (!succeeded)
                    throw new MemoryException($"ReadProcessMemory failed to read {structSize} bytes of memory from {memoryAddress.ToString("X")}");

                _localMemory.Read((IntPtr)bufferPtr, out value, marshal);
            }
        }

        /// <inheritdoc />
        public void ReadRaw(IntPtr memoryAddress, out byte[] value, int length)
        {
            value = new byte[length];
            fixed (byte* bufferPtr = value)
            {
                bool succeeded = Kernel32.Kernel32.ReadProcessMemory(_processHandle, memoryAddress, (IntPtr)bufferPtr, (UIntPtr) value.Length, out _);

                if (!succeeded)
                    throw new MemoryException($"ReadProcessMemory failed to read {value.Length} bytes of memory from {memoryAddress.ToString("X")}");
            }
        }

        /// <inheritdoc />
        public void Write<T>(IntPtr memoryAddress, ref T item) where T : unmanaged
        {
            byte[] bytes = Struct.GetBytes(ref item);

            fixed (byte* bytePtr = bytes)
            {
                bool succeeded = Kernel32.Kernel32.WriteProcessMemory(_processHandle, memoryAddress, (IntPtr)bytePtr, (UIntPtr)bytes.Length, out _);

                if (!succeeded)
                    throw new MemoryException($"WriteProcessMemory failed to write {bytes.Length} bytes of memory to {memoryAddress.ToString("X")}");
            }
        }

        /// <inheritdoc />
        public void Write<T>(IntPtr memoryAddress, ref T item, bool marshal)
        {
            byte[] bytes = Struct.GetBytes(ref item, marshal);

            fixed (byte* bytePtr = bytes)
            {
                bool succeeded = Kernel32.Kernel32.WriteProcessMemory(_processHandle, memoryAddress, (IntPtr)bytePtr, (UIntPtr) bytes.Length, out _);

                if (!succeeded)
                    throw new MemoryException($"WriteProcessMemory failed to write {bytes.Length} bytes of memory to {memoryAddress.ToString("X")}");
            }
        }

        /// <inheritdoc />
        public void WriteRaw(IntPtr memoryAddress, byte[] data)
        {
            fixed (byte* bytePtr = data)
            {
                bool succeeded = Kernel32.Kernel32.WriteProcessMemory(_processHandle, memoryAddress, (IntPtr)bytePtr, (UIntPtr) data.Length, out _);

                if (!succeeded)
                    throw new MemoryException($"WriteProcessMemory failed to write {data.Length} bytes of memory to {memoryAddress.ToString("X")}");
            }    
        }

        /// <inheritdoc />
        public IntPtr Allocate(int length)
        {
            // Call VirtualAllocEx to allocate memory of fixed chosen size.
            IntPtr returnAddress = Kernel32.Kernel32.VirtualAllocEx
            (
                _processHandle,
                IntPtr.Zero,
                (UIntPtr) length,
                Kernel32.Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
                Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );

            if (returnAddress == IntPtr.Zero)
                throw new MemoryAllocationException($"Failed to allocate memory in external process: {length} bytes, {Marshal.GetLastWin32Error()} last error.");                

            return returnAddress;
        }

        /// <inheritdoc />
        public bool Free(IntPtr address)
        {
            return Kernel32.Kernel32.VirtualFreeEx(_processHandle, address, (UIntPtr) 0, Kernel32.Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        /* Implementation */

        /// <inheritdoc />
        public Kernel32.Kernel32.MEM_PROTECTION ChangePermission(IntPtr memoryAddress, int size, Kernel32.Kernel32.MEM_PROTECTION newPermissions)
        {
            bool result = Kernel32.Kernel32.VirtualProtectEx(_processHandle, memoryAddress, (UIntPtr) size, newPermissions, out Kernel32.Kernel32.MEM_PROTECTION oldPermissions);

            if (!result)
                throw new MemoryPermissionException($"Unable to change permissions for the following memory address {memoryAddress.ToString("X")} of size {size} and permission {newPermissions.ToString()}");

            return oldPermissions;
        }
    }
}
