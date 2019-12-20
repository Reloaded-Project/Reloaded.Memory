using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Sources
{
    /// <summary>
    /// A generic extension class that extends <see cref="IMemory"/>.
    /// Provides various functions such as reading arrays.
    /// </summary>
    public static unsafe class MemoryExtensions
    {
        /* All functions are documented in the IMemory interface. */

        /*
            ----------------------
            Read Implementation(s)
            ----------------------
        */

        /* Delegates */

        /// <summary>
        /// See <see cref="IMemory.Read{T}"/>
        /// </summary>
        public delegate void ReadFunction<T> (IntPtr memoryAddress, out T value, bool marshal);

        /// <summary>
        /// See <see cref="IMemory.Write{T}"/>
        /// </summary>
        public delegate void WriteFunction<T>(IntPtr memoryAddress, ref T item, bool marshal);

        /* Read Base Implementation */

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The amount of array items to read.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void Read<T>(this IMemory memory, IntPtr memoryAddress, out T[] value, int arrayLength, bool marshal = false)
        {
            IMemory oldSource = Struct.Source;
            Struct.Source = memory;

            value = new T[arrayLength];
            StructArray.FromPtr(memoryAddress, out value, arrayLength, marshal);

            Struct.Source = oldSource;
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeRead<T>(this IMemory memory, IntPtr memoryAddress, out T value, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);

            var oldProtection = memory.ChangePermission(memoryAddress, structSize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Read(memoryAddress, out value, marshal);
            memory.ChangePermission(memoryAddress, structSize, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        public static void SafeRead<T>(this IMemory memory, IntPtr memoryAddress, out T value) where T : unmanaged
        {
            int structSize = Struct.GetSize<T>();

            var oldProtection = memory.ChangePermission(memoryAddress, structSize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Read(memoryAddress, out value);
            memory.ChangePermission(memoryAddress, structSize, oldProtection);
        }


        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads bytes from a specified memory address.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in bytes.</param>
        /// <param name="length">The amount of bytes to read from the executable.</param>
        public static void SafeReadRaw(this IMemory memory, IntPtr memoryAddress, out byte[] value, int length)
        {
            var oldProtection = memory.ChangePermission(memoryAddress, length, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);

            value = new byte[length];
            memory.ReadRaw(memoryAddress, out value, length);

            memory.ChangePermission(memoryAddress, length, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="arrayLength">The amount of array items to read.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeRead<T>(this IMemory memory, IntPtr memoryAddress, out T[] value, int arrayLength, bool marshal = false)
        {
            int regionSize = StructArray.GetSize<T>(arrayLength, marshal);

            var oldProtection = memory.ChangePermission(memoryAddress, regionSize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Read(memoryAddress, out value, arrayLength, marshal);
            memory.ChangePermission(memoryAddress, regionSize, oldProtection);
        }

        /* Write Base Implementation */

        /// <summary>
        /// Writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="items">The array of items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void Write<T>(this IMemory memory, IntPtr memoryAddress, T[] items, bool marshal = false)
        {
            IMemory oldSource = Struct.Source;
            Struct.Source = memory;

            StructArray.ToPtr(memoryAddress, items, marshal);

            Struct.Source = oldSource;
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, ref T item, bool marshal)
        {
            int memorySize = Struct.GetSize<T>(marshal);

            var oldProtection = memory.ChangePermission(memoryAddress, memorySize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Write(memoryAddress, ref item, marshal);
            memory.ChangePermission(memoryAddress, memorySize, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, ref T item) where T : unmanaged
        {
            int memorySize = Struct.GetSize<T>();

            var oldProtection = memory.ChangePermission(memoryAddress, memorySize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Write(memoryAddress, ref item);
            memory.ChangePermission(memoryAddress, memorySize, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="data">The data to write to the specified address.</param>
        public static void SafeWriteRaw(this IMemory memory, IntPtr memoryAddress, byte[] data)
        {
            var oldProtection = memory.ChangePermission(memoryAddress, data.Length, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.WriteRaw(memoryAddress, data);
            memory.ChangePermission(memoryAddress, data.Length, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="items">The array of items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, T[] items, bool marshal = false)
        {
            int regionSize = StructArray.GetSize<T>(items.Length, marshal);

            var oldProtection = memory.ChangePermission(memoryAddress, regionSize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Write(memoryAddress, items, marshal);
            memory.ChangePermission(memoryAddress, regionSize, oldProtection);
        }

        /*
            ------------
            Redirections
            ------------
        */

        /*
            Redirections simply set the default settings for the various overload shorthands.
            While it is not necessary; deriving classes may override the defaults as they wish.
        */

        /* Write: By Value to By Reference */

        /// <summary>
        /// See <see cref="IMemory.Write{T}"/>
        /// </summary>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void Write<T>    (this IMemory memory, IntPtr memoryAddress, T item, bool marshal = false) => memory.Write(memoryAddress, ref item, marshal);

        /// <summary>
        /// See <see cref="SafeWrite{T}(Reloaded.Memory.Sources.IMemory,System.IntPtr,ref T,bool)"/> />
        /// </summary>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, T item, bool marshal = false) => memory.SafeWrite(memoryAddress, ref item, marshal);

        /* ChangePermission: Size Redirections */

        /// <summary>
        /// Changes the page permissions for a specified combination of address and element from which to deduce size.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
        /// <param name="baseElement">The struct element from which the region size to change permissions for will be calculated.</param>
        /// <param name="newPermissions">The new permissions to set.</param>
        /// <param name="marshalElement">Set to true to calculate the size of the struct after marshalling instead of before.</param>
        /// <returns>The old page permissions.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage] // Wrapper that simply lets pass with base element calculated with functions tested elsewhere, no logic.
        public static Kernel32.Kernel32.MEM_PROTECTION ChangePermission<T>(this IMemory memory, IntPtr memoryAddress, ref T baseElement, Kernel32.Kernel32.MEM_PROTECTION newPermissions, bool marshalElement = false)
                           => memory.ChangePermission(memoryAddress, Struct.GetSize<T>(marshalElement), newPermissions);
    }
}
