using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Reloaded.Memory
{
    /// <summary>
    /// Utility class for working with struct arrays.
    /// </summary>
    public static unsafe class StructArray
    {
        private const int MaxStackLimit = 1024;

        /* Implementation */

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="arrayLength">The number of items to read from memory.</param>
        /// <param name="marshal">Set to true to marshal the element.</param>
        public static void FromPtr<T>(IntPtr memoryAddress, out T[] value, int arrayLength, bool marshal = false)
        {
            int structSize = Struct.GetSize<T>(marshal);
            value = new T[arrayLength];

            for (int x = 0; x < arrayLength; x++)
            {
                IntPtr address = memoryAddress + (structSize * x);
                Struct.FromPtr(address, out T result, marshal);
                value[x] = result;
            }
        }

        /// <summary>
        /// Writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">Set this to true in order to marshal the value when writing to memory.</param>
        public static void ToPtr<T>(IntPtr memoryAddress, T[] item, bool marshal = false)
        {
            int structSize = Struct.GetSize<T>(marshal);

            for (int x = 0; x < item.Length; x++)
            {
                IntPtr address = memoryAddress + (structSize * x);
                Struct.ToPtr(address, ref item[x], marshal);
            }
        }

        /// <summary>
        /// Converts a byte array to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        /// <param name="length">The amount of elements to read from the byte array.</param>
        /// <param name="startIndex">The index in the byte array to read the element(s) from.</param>
        public static void FromArray<T>(byte[] data, out T[] value, bool marshalElement, int length = 0,
            int startIndex = 0)
        {
            int structSize = Struct.GetSize<T>(marshalElement);
            int structureCount = (length == 0) ? (data.Length - startIndex) / structSize : length;
            value = new T[structureCount];

            for (int x = 0; x < value.Length; x++)
            {
                int offset = startIndex + (structSize * x);
                Struct.FromArray<T>(data, out T result, marshalElement, offset);
                value[x] = result;
            }
        }

        /// <summary>
        /// Converts a byte array to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element(s) from.</param>
        /// <param name="length">The amount of elements to read from the byte array.</param>
        public static void FromArray<T>(byte[] data, out T[] value, int startIndex = 0, int length = 0) where T : unmanaged
        {
            int structSize = Struct.GetSize<T>();
            int structureCount = (length == 0) ? (data.Length - startIndex) / structSize : length;
            value = new T[structureCount];

            for (int x = 0; x < value.Length; x++)
            {
                int offset = startIndex + (structSize * x);
                Struct.FromArray<T>(data, out T result, offset);
                value[x] = result;
            }
        }

        /// <summary>
        /// Converts a span to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="length">The amount of elements to read from the span.</param>
        public static void FromArray<T>(Span<byte> data, out T[] value, int length = 0) where T : unmanaged
        {
            int structSize     = Struct.GetSize<T>();
            int structureCount = (length == 0) ? (data.Length) / structSize : length;
            value              = new T[structureCount];

            for (int x = 0; x < value.Length; x++)
            {
                Struct.FromArray<T>(data, out value[x]);
                data = data.Slice(structSize);
            }
        }

        /// <summary>
        /// Converts a byte array to a specified Big Endian primitive.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element(s) from.</param>
        /// <param name="length">The amount of elements to read from the byte array.</param>
        public static void FromArrayBigEndianPrimitive<T>(byte[] data, out T[] value, int startIndex = 0, int length = 0) where T : unmanaged
        {
            FromArray(data, out value, startIndex, length);
            for (int x = 0; x < value.Length; x++)
                Endian.Reverse(ref value[x], out value[x]);
        }

        /// <summary>
        /// Converts a span to a specified Big Endian primitive.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="length">The amount of elements to read from the span.</param>
        public static void FromArrayBigEndianPrimitive<T>(Span<byte> data, out T[] value, int length = 0) where T : unmanaged
        {
            FromArray(data, out value, length);
            for (int x = 0; x < value.Length; x++)
                Endian.Reverse(ref value[x], out value[x]);
        }

        /// <summary>
        /// Converts a byte array to a specified Big Endian structure or class type with explicit StructLayout attribute and <see cref="IEndianReversible"/>.
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element(s) from.</param>
        /// <param name="length">The amount of elements to read from the byte array.</param>
        public static void FromArrayBigEndianStruct<T>(byte[] data, out T[] value, int startIndex = 0, int length = 0) where T : unmanaged, IEndianReversible
        {
            FromArray(data, out value, startIndex, length);
            for (int x = 0; x < value.Length; x++)
                value[x].SwapEndian();
        }

        /// <summary>
        /// Converts a span to a specified Big Endian structure or class type with explicit StructLayout attribute and <see cref="IEndianReversible"/>..
        /// </summary>
        /// <param name="value">Local variable to receive the read in struct array.</param>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="length">The amount of elements to read from the span.</param>
        public static void FromArrayBigEndianStruct<T>(Span<byte> data, out T[] value, int length = 0) where T : unmanaged, IEndianReversible
        {
            FromArray(data, out value, length);
            for (int x = 0; x < value.Length; x++)
                value[x].SwapEndian();
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="marshalElement">If set to true; will return the size of an element after marshalling.</param>
        /// <param name="elementCount">The number of array elements present.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize<T>(int elementCount, bool marshalElement)
        {
            return Struct.GetSize<T>(marshalElement) * elementCount;
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="elementCount">The number of array elements present.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize<T>(int elementCount) where T : unmanaged
        {
            return Struct.GetSize<T>() * elementCount;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        /// <param name="marshalElements">Set to true to marshal the item(s).</param>
        public static byte[] GetBytes<T>(T[] items, bool marshalElements)
        {
            int sizeOfItem  = Struct.GetSize<T>(marshalElements);
            int totalSize   = sizeOfItem * items.Length;
            var result      = new byte[totalSize];
            GetBytes(items, marshalElements, result.AsSpan());
            return result;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        /// <param name="marshalElements">Set to true to marshal the item(s).</param>
        /// <param name="buffer">The buffer to which write the bytes to.</param>
        /// <returns>The passed in buffer sliced to include only the bytes obtained.</returns>
        public static Span<byte> GetBytes<T>(T[] items, bool marshalElements, Span<byte> buffer)
        {
            int sizeOfItem = Struct.GetSize<T>(marshalElements);
            int totalSize  = sizeOfItem * items.Length;
            var resultSpan = buffer.Slice(0, totalSize);

            if (sizeOfItem < MaxStackLimit)
            {
                Span<byte> currentItem = stackalloc byte[sizeOfItem];
                GetBytesInternal(currentItem, resultSpan);
            }
            else
            {
                Span<byte> currentItem = new byte[sizeOfItem];
                GetBytesInternal(currentItem, resultSpan);
            }

            return resultSpan;

            void GetBytesInternal(Span<byte> currentItem, Span<byte> span)
            {
                for (int x = 0; x < items.Length; x++)
                {
                    Struct.GetBytes(ref items[x], marshalElements, currentItem);
                    currentItem.CopyTo(span);
                    span = span.Slice(sizeOfItem);
                }
            }
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        public static byte[] GetBytes<T>(T[] items) where T : unmanaged
        {
            int totalSize   = GetSize<T>(items.Length);
            var result      = new byte[totalSize];
            GetBytes(items, result.AsSpan());
            return result;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        /// <param name="buffer">The buffer to which write the bytes to.</param>
        /// <returns>The passed in buffer sliced to include only the bytes obtained.</returns>
        public static Span<byte> GetBytes<T>(T[] items, Span<byte> buffer) where T : unmanaged
        {
            int totalSize  = GetSize<T>(items.Length);
            var resultSpan = buffer.Slice(0, totalSize);

            if (sizeof(T) < MaxStackLimit)
            {
                Span<byte> currentItem = stackalloc byte[sizeof(T)];
                GetBytesInternal(currentItem, resultSpan);
            }
            else
            {
                Span<byte> currentItem = new byte[sizeof(T)];
                GetBytesInternal(currentItem, resultSpan);
            }

            return resultSpan;

            void GetBytesInternal(Span<byte> currentItem, Span<byte> span)
            {
                for (int x = 0; x < items.Length; x++)
                {
                    Struct.GetBytes(ref items[x], currentItem);
                    currentItem.CopyTo(span);
                    span = span.Slice(sizeof(T));
                }
            }
        }
    }
}
