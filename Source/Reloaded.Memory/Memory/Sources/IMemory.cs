using System;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    public interface IMemory
    {
        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in struct.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        void Read<T>       (IntPtr memoryAddress, out T value, bool marshal = false);

        /// <summary>
        /// Reads raw data from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="value">Local variable to receive the read in bytes.</param>
        /// <param name="length">The amount of bytes to read starting from the <see cref="memoryAddress"/>.</param>
        void ReadRaw(IntPtr memoryAddress, out byte[] value, int length);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        void Write<T>       (IntPtr memoryAddress, ref T item, bool marshal = false);

        /// <summary>
        /// Writes raw data to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="data">The bytes to write to <see cref="memoryAddress"/>.</param>
        void WriteRaw       (IntPtr memoryAddress, byte[] data);

        /// <summary>
        /// Allocates fixed size of memory inside the target memory source. 
        /// Returns the address of newly allocated memory. 
        /// </summary>
        /// <param name="length">Amount of bytes to be allocated.</param>
        /// <returns>Address to the newly allocated memory.</returns>
        IntPtr Allocate     (int length);

        /// <summary>
        /// Frees memory previously allocated with <see cref="Allocate"/>.
        /// </summary>
        /// <param name="address">The address of the memory to free.</param>
        /// <returns>True if the operation is successful.</returns>
        bool Free           (IntPtr address);

        /// <summary>
        /// Changes the page permissions for a specified combination of address and length.
        /// </summary>
        /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
        /// <param name="size">The region size for which to change permissions for.</param>
        /// <param name="newPermissions">The new permissions to set.</param>
        /// <returns>The old page permissions.</returns>
        Kernel32.MEM_PROTECTION ChangePermission     (IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions);
    }
}
