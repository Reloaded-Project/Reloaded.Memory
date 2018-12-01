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

        public void    Read<T>(IntPtr memoryAddress, out T value, bool marshal = false)
        {
            value = marshal ? Marshal.PtrToStructure<T>(memoryAddress) : Unsafe.Read<T>((void*)memoryAddress);
        }

        public void    ReadRaw(IntPtr memoryAddress, out byte[] value, int length)
        {
            value = new byte[length];
            Marshal.Copy(memoryAddress, value, 0, value.Length);
        }

        public void    Write<T>(IntPtr memoryAddress, ref T item, bool marshal = false)
        {
            if (marshal)
                Marshal.StructureToPtr(item, memoryAddress, false);
            else
                Unsafe.Write((void*)memoryAddress, item);
        }

        public void    WriteRaw(IntPtr memoryAddress, byte[] data)
        {
            Marshal.Copy(data, 0, memoryAddress, data.Length);
        }

        public IntPtr  Allocate(int length)
        {
            return Marshal.AllocHGlobal(length);
        }

        public bool    Free(IntPtr address)
        {
            Marshal.FreeHGlobal(address);
            return true;
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        public Kernel32.MEM_PROTECTION ChangePermission(IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions)
        {
            bool result = Kernel32.VirtualProtect(memoryAddress, (uint)size, newPermissions, out var oldPermissions);

            if (!result)
                throw new PermissionChangeFailureException($"Unable to change permissions for the following memory address {memoryAddress.ToString("X")} of size {size} and permission {newPermissions.ToString()}");

            return oldPermissions;
        }
    }
}
