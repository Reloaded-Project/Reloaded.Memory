using System;
using System.Diagnostics;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
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
        private IntPtr _processHandle;
        
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
        public ExternalMemory(Process process)
        {
            _processHandle  = process.Handle;
        }


        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        public void Read<T>(IntPtr memoryAddress, out T value, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);
            byte[] buffer  = new byte[structSize];

            fixed (byte* bufferPtr = buffer)
            {
                Kernel32.ReadProcessMemory(_processHandle, memoryAddress, (IntPtr)bufferPtr, (uint)structSize, out var _);
                Struct.FromPtr((IntPtr)bufferPtr, out value, _localMemory.Read, marshal);
            }
        }

        public void ReadRaw(IntPtr memoryAddress, out byte[] value, int length)
        {
            value = new byte[length];
            fixed (byte* bufferPtr = value)
            {
                Kernel32.ReadProcessMemory(_processHandle, memoryAddress, (IntPtr)bufferPtr, (uint)value.Length, out var _);
            }
        }

        public void Write<T>(IntPtr memoryAddress, ref T item, bool marshal)
        {
            byte[] bytes = Struct.GetBytes(ref item, marshal);

            fixed (byte* bytePtr = bytes)
                Kernel32.WriteProcessMemory(_processHandle, memoryAddress, (IntPtr)bytePtr, (uint)bytes.Length, out var _);
        }

        public void WriteRaw(IntPtr memoryAddress, byte[] data)
        {
            fixed (byte* bytePtr = data)
                Kernel32.WriteProcessMemory(_processHandle, memoryAddress, (IntPtr)bytePtr, (uint)data.Length, out var _);
        }

        public IntPtr Allocate(int length)
        {
            // Call VirtualAllocEx to allocate memory of fixed chosen size.
            return Kernel32.VirtualAllocEx
            (
                _processHandle,
                IntPtr.Zero,
                (uint)length,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
                Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );
        }

        public bool Free(IntPtr address)
        {
            return Kernel32.VirtualFreeEx(_processHandle, address, 0, Kernel32.MEM_ALLOCATION_TYPE.MEM_DECOMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        /* Implementation */
        public Kernel32.MEM_PROTECTION ChangePermission(IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions)
        {
            Kernel32.VirtualProtectEx(_processHandle, memoryAddress, (uint)size, newPermissions, out Kernel32.MEM_PROTECTION oldPermissions);
            return oldPermissions;
        }
    }
}
