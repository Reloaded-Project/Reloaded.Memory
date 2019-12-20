using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// An interface type which describes a pointer to an array in arbitrary memory.
    /// </summary>
    /// <typeparam name="TStruct">A primitive or struct type to which the pointer is intended to point to.</typeparam>
    public interface IArrayPtr<TStruct>
    {
        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        int ElementSize { get; }

        /// <summary>
        /// If this is true; elements will be marshaled as they are read in and out from memory.
        /// </summary>
        bool MarshalElements { get; set; }

        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="IArrayPtr{T}"/>.
        /// </summary>
        unsafe void* Pointer { get; set; }

        /// <summary>
        /// The source where memory will be read/written to/from.
        /// </summary>
        IMemory Source { get; set; }

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index">The index to retrieve a pointer for.</param>
        /// <returns>Pointer to the requested element at index.</returns>
        unsafe void* GetPointerToElement(int index);

        /// <summary>
        /// Gets the value of an item at a specific index.
        /// </summary>
        /// <param name="value">The value to be received from the array.</param>
        /// <param name="index">The index in the array from which to receive the value.</param>
        void Get(out TStruct value, int index);

        /// <summary>
        /// Sets the value of an item at a specific index.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        /// <param name="index">The index in the array to which the value is to be written to.</param>
        void Set(ref TStruct value, int index);
        
        /// <summary/>
        TStruct this[int index] { get; set; }
    }
}