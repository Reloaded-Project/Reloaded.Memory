using System.Collections;
using Reloaded.Memory.Interfaces;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Reloaded.Memory.Pointers.Sourced;

/// <summary>
///     A variant of <see cref="FixedArrayPtr{T}" />, which has an attached source.
/// </summary>
/// <typeparam name="T">Type of struct behind this pointer.</typeparam>
/// <typeparam name="TSource">The source the operations on the pointer are performed on.</typeparam>
public unsafe struct SourcedFixedArrayPtr<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
    T, TSource> : IEnumerable<T> where T : unmanaged where TSource : ICanReadWriteMemory
{
    /// <summary>
    ///     The pointer to the value.
    ///     For reference only; only use for pointers in RAM (based on <see cref="Memory{T}" />), otherwise use methods!
    /// </summary>
    /// <remarks>Only use for pointers in same process.</remarks>
    public FixedArrayPtr<T> Pointer;

    /// <summary>
    ///     The source from which the data is read/written from.
    /// </summary>
    public TSource Source;

    /// <summary>
    ///     The number of elements contained in the <see cref="FixedArrayPtr{T}" />.
    /// </summary>
    public int Count
    {
        get => Pointer.Count;
        set => Pointer.Count = value;
    }

    /// <summary>
    ///     Contains the size of the entire array, in bytes.
    /// </summary>
    public int ArraySize => Count * sizeof(T);

    /// <summary>
    ///     Constructs a new instance of <see cref="SourcedFixedArrayPtr{T,TSource}" /> given the address of the first element,
    ///     and the number of elements that follow it; as well as a source where to read from.
    /// </summary>
    /// <param name="address">The address of the first element of the structure array.</param>
    /// <param name="count">The amount of elements in the array structure in memory.</param>
    /// <param name="source">Source where to pull elements from.</param>
    public SourcedFixedArrayPtr(T* address, int count, TSource source)
    {
        Pointer = new FixedArrayPtr<T>(address, count);
        Source = source;
    }

    /// <summary>
    ///     Converts the specified element of the <see cref="FixedArrayPtr{T}" /> to a reference.
    /// </summary>
    /// <param name="index">The index of the element to get the reference of.</param>
    /// <returns>A reference to the element at the specified index.</returns>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T AsRef(int index) => ref Pointer.AsRef(index);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset.
    /// </summary>
    /// <param name="index">The index offset of the element.</param>
    /// <returns>The value at the pointer's address plus the index offset.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get(int index) => Pointer.Get(Source, index);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to.
    /// </summary>
    /// <param name="value">The value at the pointer's address.</param>
    public void Get(out T value) => Pointer.Get(Source, out value);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset.
    /// </summary>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value at the pointer's address plus the index offset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get(int index, out T value) => Pointer.Get(Source, index, out value);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to.
    /// </summary>
    /// <param name="value">The value to set at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(in T value) => Pointer.Set(Source, value);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to plus the index offset.
    /// </summary>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value to set at the pointer's address plus the index offset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int index, in T value) => Pointer.Set(Source, index, value);

    /// <summary>
    ///     Determines whether an element is in the <see cref="FixedArrayPtr{T}" />.
    /// </summary>
    /// <param name="item">The item to determine if it is contained in the collection.</param>
    /// <returns>Whether the item is in the collection or not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(in T item) => Pointer.Contains(Source, item);

    /// <summary>
    ///     Searches for a specified item and returns the index of the item if present.
    /// </summary>
    /// <param name="item">The item to search for in the array.</param>
    /// <returns>The index of the item, if present in the array, else -1.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(in T item) => Pointer.IndexOf(Source, item);

    /// <summary>
    ///     Copies elements from the source array to the FixedArrayPtr.
    /// </summary>
    /// <param name="sourceArray">The source array to copy elements from.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyFrom(Span<T> sourceArray, int length) => Pointer.CopyFrom(Source, sourceArray, length);

    /// <summary>
    ///     Copies elements from the source array to the FixedArrayPtr.
    /// </summary>
    /// <param name="sourceArray">The source array to copy elements from.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <param name="sourceIndex">The index in the source array to start copying from. Default is 0.</param>
    /// <param name="destinationIndex">The index in the FixedArrayPtr to start copying to. Default is 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyFrom(Span<T> sourceArray, int length, int sourceIndex, int destinationIndex)
        => Pointer.CopyFrom(Source, sourceArray, length, sourceIndex, destinationIndex);

    /// <summary>
    ///     Copies elements from the FixedArrayPtr to the destination array.
    /// </summary>
    /// <param name="destinationArray">The destination array to copy elements to.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyTo(Span<T> destinationArray, int length) => Pointer.CopyTo(Source, destinationArray, length);

    /// <summary>
    ///     Copies elements from the FixedArrayPtr to the destination array.
    /// </summary>
    /// <param name="destinationArray">The destination array to copy elements to.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <param name="sourceIndex">The index in the FixedArrayPtr to start copying from. Default is 0.</param>
    /// <param name="destinationIndex">The index in the destination array to start copying to. Default is 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyTo(Span<T> destinationArray, int length, int sourceIndex, int destinationIndex)
        => Pointer.CopyTo(Source, destinationArray, length, sourceIndex, destinationIndex);

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        var count = Count;
        for (var x = 0; x < count; x++)
            yield return Get(x);
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
