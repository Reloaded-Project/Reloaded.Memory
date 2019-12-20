using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Reloaded.Memory.Sources;
using MemoryExtensions = Reloaded.Memory.Sources.MemoryExtensions;

namespace Reloaded.Memory
{
    /// <summary>
    /// Struct is a general utility class providing functions which provides various functions for working with structures; such
    /// as reading/writing to/from memory of structures.
    /// </summary>
    public static unsafe class Struct
    {
        /* Memory Sources */

        /// <summary>
        /// Defines the source for the default memory reading and writing <see cref="ToPtr{T}(System.IntPtr,T,bool)"/> and <see cref="FromPtr{T}(System.IntPtr,out T,bool)"/> functions.
        /// This also affects the <see cref="StructArray"/> class.
        /// </summary>
        public static IMemory Source { get; set; } = new Sources.Memory();

        /// <summary>
        /// Allows for access of memory of this individual process.
        /// </summary>
        private static IMemory _thisProcessMemory = new Sources.Memory();

        /* Redirections/Shorthands */

        /* ToPtr: Default Setting Shorthands */

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">T</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage] // Proxy Overload
        public static void ToPtr<T>(IntPtr pointer, T item, bool marshalElement = false) => ToPtr(pointer, ref item, marshalElement);

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">The item to write to a specified pointer.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ExcludeFromCodeCoverage] // Proxy Overload
        public static void ToPtr<T>(IntPtr pointer, ref T item, bool marshalElement = false) => ToPtr(pointer, ref item, Source.Write, marshalElement);

        /* FromPtr: Default Setting Shorthands */

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        [ExcludeFromCodeCoverage] // Proxy Overload (Source.Read cannot be set default)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FromPtr<T>(IntPtr pointer, out T value, bool marshalElement = false) => FromPtr(pointer, out value, Source.Read<T>, marshalElement);


        /* Implementation */

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">The item to write to a specified pointer.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        /// <param name="writeFunction">The function to use that writes data to memory given a pointer, item, type and marshal option.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToPtr<T>(IntPtr pointer, ref T item, MemoryExtensions.WriteFunction<T> writeFunction, bool marshalElement = false) => writeFunction(pointer, ref item, marshalElement);

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="readFunction">A function that reads data from memory given a pointer, type and marshal option.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FromPtr<T>(IntPtr pointer, out T value, MemoryExtensions.ReadFunction<T> readFunction, bool marshalElement = false) => readFunction(pointer, out value, marshalElement);

        /// <summary>
        /// Converts a byte array to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        /// <param name="startIndex">The index in the byte array to read the element from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FromArray<T>(byte[] data, out T value, bool marshalElement, int startIndex = 0)
        {
            fixed (byte* dataPtr = data)
            {
                _thisProcessMemory.Read((IntPtr)(&dataPtr[startIndex]), out value, marshalElement);
            }
        }

        /// <summary>
        /// Converts a byte array to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FromArray<T>(byte[] data, out T value, int startIndex = 0) where T : unmanaged
        {
            var arraySpan = new Span<byte>(data, startIndex, data.Length - startIndex);
            value = MemoryMarshal.Read<T>(arraySpan);
        }

        /// <summary>
        /// Converts a span to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FromArray<T>(Span<byte> data, out T value) where T : unmanaged
        {
            value = MemoryMarshal.Read<T>(data);
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="marshalElement">If set to true; will return the size of an element after marshalling.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize<T>(bool marshalElement)
        {
            return marshalElement ? Marshal.SizeOf<T>() : Unsafe.SizeOf<T>();
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize<T>() where T : unmanaged
        {
            return Unsafe.SizeOf<T>();
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        public static byte[] GetBytes<T>(ref T item) where T : unmanaged
        {
            int size = sizeof(T);
            byte[] array = new byte[size];

            var arraySpan = new Span<byte>(array);
            MemoryMarshal.Write(arraySpan, ref item);

            return array;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static byte[] GetBytes<T>(ref T item, bool marshalElement)
        {
            int size = GetSize<T>(marshalElement);
            byte[] array = new byte[size];

            fixed (byte* arrayPtr = array)
            {
                _thisProcessMemory.Write((IntPtr)arrayPtr, ref item, marshalElement);
            }

            return array;
        }
    }
}
