using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Sources
{
    /// <summary>
    /// A generic extension class that extends <see cref="IMemory"/>.
    /// Provides various functions such as reading arrays.
    /// </summary>
    public static class MemoryExtensions
    {
        /* All functions are documented in the IMemory interface. */

        /*
            ----------------------
            Read Implementation(s)
            ----------------------
        */

        /* Delegates */

        /// <summary>
        /// See <see cref="IMemory.Read{T}(nuint,out T)"/>
        /// </summary>
        public delegate void ReadFunction<T> (nuint memoryAddress, out T value, bool marshal);

        /// <summary>
        /// See <see cref="IMemory.Write{T}(nuint,ref T)"/>
        /// </summary>
        public delegate void WriteFunction<T>(nuint memoryAddress, ref T item, bool marshal);

        /* Read Base Implementation */

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The amount of array items to read.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void Read<TMemory,
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TMemory memory, nuint memoryAddress, out T[] value, int arrayLength, bool marshal = false) where TMemory : IMemory
        {
            IMemory oldSource = Struct.Source;
            Struct.Source = memory;

#if NET5_0_OR_GREATER
            value = GC.AllocateUninitializedArray<T>(arrayLength, false);
#else
            value = new T[arrayLength];
#endif
            StructArray.FromPtr(memoryAddress, out value, arrayLength, marshal);

            Struct.Source = oldSource;
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeRead<TMemory,
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TMemory memory, nuint memoryAddress, out T value, bool marshal) where TMemory : IMemory
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
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        public static void SafeRead<TMemory, T>(this TMemory memory, nuint memoryAddress, out T value) where T : unmanaged where TMemory : IMemory
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
        public static void SafeReadRaw<TMemory>(this TMemory memory, nuint memoryAddress, out byte[] value, int length) where TMemory : IMemory
        {
            var oldProtection = memory.ChangePermission(memoryAddress, length, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);

#if NET5_0_OR_GREATER
            value = GC.AllocateUninitializedArray<byte>(length, false);
#else
            value = new byte[length];
#endif
            memory.ReadRaw(memoryAddress, out value, length);

            memory.ChangePermission(memoryAddress, length, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="arrayLength">The amount of array items to read.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeRead<TMemory,
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TMemory memory, nuint memoryAddress, out T[] value, int arrayLength, bool marshal = false) where TMemory : IMemory
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
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="items">The array of items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void Write<TMemory, T>(this TMemory memory, nuint memoryAddress, T[] items, bool marshal = false) where TMemory : IMemory
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
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeWrite<TMemory, T>(this TMemory memory, nuint memoryAddress, ref T item, bool marshal) where TMemory : IMemory
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
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        public static void SafeWrite<TMemory, T>(this TMemory memory, nuint memoryAddress, ref T item) where T : unmanaged where TMemory : IMemory
        {
            int memorySize = Struct.GetSize<T>();

            var oldProtection = memory.ChangePermission(memoryAddress, memorySize, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Write(memoryAddress, ref item);
            memory.ChangePermission(memoryAddress, memorySize, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="data">The data to write to the specified address.</param>
        public static void SafeWriteRaw<TMemory>(this TMemory memory, nuint memoryAddress, byte[] data) where TMemory : IMemory
        {
            var oldProtection = memory.ChangePermission(memoryAddress, data.Length, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.WriteRaw(memoryAddress, data);
            memory.ChangePermission(memoryAddress, data.Length, oldProtection);
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="items">The array of items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeWrite<TMemory, T>(this TMemory memory, nuint memoryAddress, T[] items, bool marshal = false) where TMemory : IMemory
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

        /* Read: Using long instead of IntPtr */

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory">Memory instance to read from.</param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<TMemory, T>(this TMemory memory, int memoryAddress) where T : unmanaged where TMemory : IMemory
        {
            memory.Read<T>((nuint)memoryAddress, out var result);
            return result;
        }

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory">Memory instance to read from.</param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="marshal">Set true to marshal memory, else false.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<TMemory,
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TMemory memory, int memoryAddress, bool marshal) where TMemory : IMemory
        {
            memory.Read<T>((nuint)memoryAddress, out var result, marshal);
            return result;
        }

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory">Memory instance to read from.</param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<TMemory, T>(this TMemory memory, long memoryAddress) where T : unmanaged where TMemory : IMemory
        {
            memory.Read<T>((nuint)memoryAddress, out var result);
            return result;
        }

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory">Memory instance to read from.</param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="marshal">Set true to marshal memory, else false.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<TMemory,
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
            T>(this TMemory memory, long memoryAddress, bool marshal) where TMemory : IMemory
        {
            memory.Read<T>((nuint)memoryAddress, out var result, marshal);
            return result;
        }

        /* Read: By Value to By Reference */

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory">Memory instance to read from.</param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<TMemory, T>(this TMemory memory, nuint memoryAddress) where T : unmanaged where TMemory : IMemory
        {
            memory.Read<T>(memoryAddress, out var result);
            return result;
        }

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory">Memory instance to read from.</param>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="marshal">Set true to marshal memory, else false.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<TMemory,
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TMemory memory, nuint memoryAddress, bool marshal) where TMemory : IMemory
        {
            memory.Read<T>(memoryAddress, out var result, marshal);
            return result;
        }

        /* Write: Using int/long instead of IntPtr */

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<TMemory, T>(this TMemory memory, int memoryAddress, T item) where T : unmanaged where TMemory : IMemory => memory.Write<T>((nuint)memoryAddress, ref item);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">True to marshal the element, else false.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void Write<TMemory, T>(this TMemory memory, int memoryAddress, T item, bool marshal = false) where TMemory : IMemory => memory.Write((nuint)memoryAddress, ref item, marshal);

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void SafeWrite<TMemory, T>(this TMemory memory, int memoryAddress, T item, bool marshal = false) where TMemory : IMemory => memory.SafeWrite((nuint)memoryAddress, ref item, marshal);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<TMemory, T>(this TMemory memory, long memoryAddress, T item) where T : unmanaged where TMemory : IMemory => memory.Write<T>((nuint)memoryAddress, ref item);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">True to marshal the element, else false.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void Write<TMemory, T>(this TMemory memory, long memoryAddress, T item, bool marshal = false) where TMemory : IMemory => memory.Write((nuint)memoryAddress, ref item, marshal);

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void SafeWrite<TMemory, T>(this TMemory memory, long memoryAddress, T item, bool marshal = false) where TMemory : IMemory => memory.SafeWrite((nuint)memoryAddress, ref item, marshal);

        /* Write: By Value to By Reference */

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<TMemory, T>(this TMemory memory, nuint memoryAddress, T item) where T : unmanaged where TMemory : IMemory => memory.Write<T>(memoryAddress, ref item);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">True to marshal the element, else false.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void Write<TMemory, T>(this TMemory memory, nuint memoryAddress, T item, bool marshal = false) where TMemory : IMemory => memory.Write(memoryAddress, ref item, marshal);

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        [ExcludeFromCodeCoverage] // This is a wrapper that simply lets pass by value, no logic.
        public static void SafeWrite<TMemory, T>(this TMemory memory, nuint memoryAddress, T item, bool marshal = false) where TMemory : IMemory => memory.SafeWrite(memoryAddress, ref item, marshal);

        /* ChangePermission: Size Redirections */

        /// <summary>
        /// Changes the page permissions for a specified combination of address and element from which to deduce size.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <typeparam name="TMemory">Type which inherits from <see cref="IMemory"/>.</typeparam>
        /// <param name="memory"></param>
        /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
        /// <param name="baseElement">The struct element from which the region size to change permissions for will be calculated.</param>
        /// <param name="newPermissions">The new permissions to set.</param>
        /// <param name="marshalElement">Set to true to calculate the size of the struct after marshalling instead of before.</param>
        /// <returns>The old page permissions.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage] // Wrapper that simply lets pass with base element calculated with functions tested elsewhere, no logic.
        public static Kernel32.Kernel32.MEM_PROTECTION ChangePermission<TMemory, T>(this TMemory memory, nuint memoryAddress, ref T baseElement, Kernel32.Kernel32.MEM_PROTECTION newPermissions, bool marshalElement = false) 
            where TMemory : IMemory => memory.ChangePermission(memoryAddress, Struct.GetSize<T>(marshalElement), newPermissions);
    }
}
