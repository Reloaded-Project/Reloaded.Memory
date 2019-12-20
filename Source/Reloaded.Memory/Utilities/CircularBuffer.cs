using System;
using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Utilities
{
    /// <summary>
    /// The <see cref="CircularBuffer"/> is a writeable buffer useful for temporary storage of data.
    /// It's a buffer whereby once you reach the end of the buffer, it loops back over to the beginning of the stream automatically.
    /// </summary>
    public class CircularBuffer : IDisposable
    {
        /// <summary>
        /// Access to memory source where the <see cref="CircularBuffer"/> reads from/writes to.
        /// </summary>
        public IMemory  Source    { get; }

        /// <summary>
        /// Current offset of next item within the <see cref="CircularBuffer"/>.
        /// </summary>
        public int      Offset    { get; set; }

        /// <summary>
        /// The address of the <see cref="CircularBuffer"/>.
        /// </summary>
        public IntPtr   Address   { get; set; }

        /// <summary>
        /// Size of the <see cref="CircularBuffer"/>.
        /// </summary>
        public int      Size      { get; set; }

        /// <summary>
        /// Returns the address of where the next element will be written onto the buffer.
        /// </summary>
        public IntPtr   WritePointer => Address + Offset;

        /// <summary> Remaining space until the Circular buffer next loops. </summary>
        private int     Remaining    => Size - Offset;

        /// <summary>
        /// Creates a <see cref="CircularBuffer"/> within the target memory source.
        /// </summary>
        /// <param name="size">The size of the buffer in bytes.</param>
        /// <param name="source">The source of the buffer where to read/write elements to/from.</param>
        public CircularBuffer(int size, IMemory source)
        {
            Offset      = 0;
            Size        = size;
            Source      = source;
            Address     = source.Allocate(Size);
        }

        /// <summary>
        /// Destroys the current class instance.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~CircularBuffer()
        {
            Dispose();
        }

        /* Public Interface */

        /// <summary>
        /// Adds a new item onto the circular buffer.
        /// </summary>
        /// <param name="bytesToWrite">The bytes to add onto the buffer.</param>
        /// <returns>Pointer to the recently added item to the buffer, or IntPtr.Zero if the item cannot fit.</returns>
        public IntPtr Add(byte[] bytesToWrite)
        {
            var canFit = CanItemFit(bytesToWrite.Length);

            if (canFit == ItemFit.No)
                return IntPtr.Zero;

            if (canFit == ItemFit.StartOfBuffer)
                Offset = 0;

            IntPtr writeAddress = WritePointer;
            Source.WriteRaw(writeAddress, bytesToWrite);

            Offset += bytesToWrite.Length;
            return writeAddress;
        }

        /// <summary>
        /// Adds a new item onto the circular buffer.
        /// </summary>
        /// <param name="item">The item to add onto the buffer.</param>
        /// <param name="marshalElement">The element to be marshalled.</param>
        /// <returns>Pointer to the recently added item to the buffer.</returns>
        public IntPtr Add<TStructure>(ref TStructure item, bool marshalElement = false)
        {
            return Add(Struct.GetBytes(ref item, marshalElement));
        }

        /// <summary>
        /// Returns an enumerable describing if an item can fit into the buffer.
        /// </summary>
        /// <param name="objectSize">The size of the object to be appended to the buffer.</param>
        public ItemFit CanItemFit(int objectSize)
        { 
            if (Remaining >= objectSize)
                return ItemFit.Yes;

            if (Size >= objectSize)
                return ItemFit.StartOfBuffer;

            return ItemFit.No;
        }

        /// <summary>
        /// Returns an enumerable describing if an item can fit into the buffer.
        /// </summary>
        /// <param name="item">The item to check if it can fit into the buffer.</param>
        /// <param name="marshalElement">True if the item is to be marshalled, else false.</param>
        public ItemFit CanItemFit<TStructure>(ref TStructure item, bool marshalElement = false)
        {
            return CanItemFit(Struct.GetSize<TStructure>(marshalElement));
        }

        /* Custom Types. */

        /// <summary/>
        public enum ItemFit
        {
            /// <summary> The item is too large to fit into the buffer. </summary>
            No,
            /// <summary> The item can fit into the buffer. </summary>
            Yes,
            /// <summary> The item can fit into the buffer, but not in the remaining space (will be placed at start of buffer).</summary>
            StartOfBuffer
        }

        /* Overrides. */

        /// <summary/>
        public void Dispose()
        {
            Source.Free(Address);
            GC.SuppressFinalize(this);
        }
    }
}
