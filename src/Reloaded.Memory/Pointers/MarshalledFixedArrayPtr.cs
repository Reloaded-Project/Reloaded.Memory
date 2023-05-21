using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Pointers;

/// <summary>
///     A variant of <see cref="MarshalledPtr{T}" />, except with known fixed size.
///     This is for pointers at a specific fixed memory location and a specific size, like static data in an executable.
/// </summary>
/// <typeparam name="T">Type of struct behind this pointer.</typeparam>
public unsafe struct MarshalledFixedArrayPtr<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
    T> where T : new()
{
    /// <summary>
    ///     Pointer to the first element of the array.
    /// </summary>
    public MarshalledPtr<T> Pointer;

    /// <summary>
    ///     The number of elements contained in the <see cref="FixedArrayPtr{T}" />.
    /// </summary>
    public int Count;

    /// <summary>
    ///     Contains the size of the entire array, in bytes.
    /// </summary>
    public int ArraySize => Count * Pointer.ElementSize;

    /// <summary>
    ///     Constructs a new instance of <see cref="FixedArrayPtr{T}" /> given the address of the first element,
    ///     and the number of elements that follow it.
    /// </summary>
    /// <param name="address">The address of the first element of the structure array.</param>
    /// <param name="count">The amount of elements in the array structure in memory.</param>
    public MarshalledFixedArrayPtr(byte* address, int count)
    {
        Pointer = new MarshalledPtr<T>(address);
        Count = count;
    }

    /// <summary>
    ///     Gets the value of the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to get the value of.</param>
    /// <returns>The value of the element at the specified index.</returns>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get(int index) => Pointer.Get(index);

    /// <summary>
    ///     Gets the value of the element.
    /// </summary>
    /// <returns>The value of the element.</returns>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get() => Pointer.Get();

    /// <summary>
    ///     Gets the value of the element.
    /// </summary>
    /// <param name="value">The value of the element at the specified index.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get(out T value) => Pointer.Get(out value!);

    /// <summary>
    ///     Gets the value of the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to get the value of.</param>
    /// <param name="value">The value of the element at the specified index.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get(int index, out T value) => Pointer.Get(index, out value!);

    /// <summary>
    ///     Sets the value of the element.
    /// </summary>
    /// <param name="value">The value to set</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(in T value) => Pointer.Set(value!);

    /// <summary>
    ///     Sets the value of the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to set the value of.</param>
    /// <param name="value">The value to set at the specified index.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int index, in T value) => Pointer.Set(index, value!);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <returns>The value at the pointer's address.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<TSource>(TSource source) where TSource : ICanReadWriteMemory => Pointer.Get(source);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="value">The value at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get<TSource>(TSource source, out T value) where TSource : ICanReadWriteMemory
        => Pointer.Get(source, out value!);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to write to.</param>
    /// <param name="value">The value to set at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<TSource>(TSource source, in T value) where TSource : ICanReadWriteMemory
        => Pointer.Set(source, value!);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset from a given
    ///     <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="index">The index offset of the element.</param>
    /// <returns>The value at the pointer's address plus the index offset.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<TSource>(TSource source, int index) where TSource : ICanReadWriteMemory => Pointer.Get(source, index);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset from a given
    ///     <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value at the pointer's address plus the index offset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get<TSource>(TSource source, int index, out T value) where TSource : ICanReadWriteMemory
        => Pointer.Get(source, index, out value!);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to plus the index offset from a given
    ///     <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to write to.</param>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value to set at the pointer's address plus the index offset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<TSource>(TSource source, int index, [DisallowNull] in T value) where TSource : ICanReadWriteMemory
        => Pointer.Set(source, index, value);

    /// <summary>
    ///     Determines whether an element is in the <see cref="FixedArrayPtr{T}" />.
    /// </summary>
    /// <param name="item">The item to determine if it is contained in the collection.</param>
    /// <returns>Whether the item is in the collection or not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(in T item) => IndexOf(item) != -1;

    /// <summary>
    ///     Determines whether an element is in the <see cref="FixedArrayPtr{T}" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="item">The item to determine if it is contained in the collection.</param>
    /// <returns>Whether the item is in the collection or not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains<TSource>(TSource source, in T item) where TSource : ICanReadWriteMemory
        => IndexOf(source, item) != -1;

    /// <summary>
    ///     Searches for a specified item and returns the index of the item
    ///     if present.
    /// </summary>
    /// <param name="item">The item to search for in the array.</param>
    /// <returns>The index of the item, if present in the array, else -1.</returns>
    public int IndexOf(in T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (var x = 0; x < Count; x++)
        {
            if (comparer.Equals(item, Get(x)))
                return x;
        }

        return -1;
    }

    /// <summary>
    ///     Searches for a specified item and returns the index of the item
    ///     if present.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="item">The item to search for in the array.</param>
    /// <returns>The index of the item, if present in the array, else -1.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf<TSource>(TSource source, in T item) where TSource : ICanReadWriteMemory
    {
        var comparer = EqualityComparer<T>.Default;
        for (var x = 0; x < Count; x++)
        {
            if (comparer.Equals(item, Get(source, x)))
                return x;
        }

        return -1;
    }

    /// <summary>
    ///     Copies elements from the source array to the FixedArrayPtr.
    /// </summary>
    /// <param name="sourceArray">The source array to copy elements from.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyFrom(Span<T> sourceArray, int length) => CopyFrom(sourceArray, length, 0, 0);

    /// <summary>
    ///     Copies elements from the source array to the FixedArrayPtr.
    /// </summary>
    /// <param name="sourceArray">The source array to copy elements from.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <param name="sourceIndex">The index in the source array to start copying from. Default is 0.</param>
    /// <param name="destinationIndex">The index in the FixedArrayPtr to start copying to. Default is 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyFrom(Span<T> sourceArray, int length, int sourceIndex, int destinationIndex)
    {
        if (length < 0 || sourceIndex < 0 || sourceIndex + length > sourceArray.Length || destinationIndex < 0 ||
            destinationIndex + length > Count)
            ThrowHelpers.ThrowArgumentOutOfRangeException(length, sourceIndex, destinationIndex, sourceArray.Length,
                Count);

        for (var x = 0; x < length; x++)
            Set(destinationIndex + x, sourceArray[sourceIndex + x]);
    }

    /// <summary>
    ///     Copies elements from the FixedArrayPtr to the destination array.
    /// </summary>
    /// <param name="destinationArray">The destination array to copy elements to.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyTo(Span<T> destinationArray, int length) => CopyTo(destinationArray, length, 0, 0);

    /// <summary>
    ///     Copies elements from the FixedArrayPtr to the destination array.
    /// </summary>
    /// <param name="destinationArray">The destination array to copy elements to.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <param name="sourceIndex">The index in the FixedArrayPtr to start copying from. Default is 0.</param>
    /// <param name="destinationIndex">The index in the destination array to start copying to. Default is 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyTo(Span<T> destinationArray, int length, int sourceIndex, int destinationIndex)
    {
        if (length < 0 || sourceIndex < 0 || sourceIndex + length > Count || destinationIndex < 0 ||
            destinationIndex + length > destinationArray.Length)
            ThrowHelpers.ThrowArgumentOutOfRangeException(length, sourceIndex, destinationIndex,
                destinationArray.Length, Count);

        for (var x = 0; x < length; x++)
            destinationArray[destinationIndex + x] = Get(sourceIndex + x);
    }

    /// <summary>
    ///     Copies elements from the source array to the FixedArrayPtr.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read/write from.</param>
    /// <param name="sourceArray">The source array to copy elements from.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyFrom<TSource>(TSource source, Span<T> sourceArray, int length) where TSource : ICanReadWriteMemory
        => CopyFrom(source, sourceArray, length, 0, 0);

    /// <summary>
    ///     Copies elements from the source array to the FixedArrayPtr.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read/write from.</param>
    /// <param name="sourceArray">The source array to copy elements from.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <param name="sourceIndex">The index in the source array to start copying from. Default is 0.</param>
    /// <param name="destinationIndex">The index in the FixedArrayPtr to start copying to. Default is 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyFrom<TSource>(TSource source, Span<T> sourceArray, int length, int sourceIndex,
        int destinationIndex) where TSource : ICanReadWriteMemory
    {
        if (length < 0 || sourceIndex < 0 || sourceIndex + length > sourceArray.Length || destinationIndex < 0 ||
            destinationIndex + length > Count)
            ThrowHelpers.ThrowArgumentOutOfRangeException(length, sourceIndex, destinationIndex, sourceArray.Length,
                Count);

        for (var x = 0; x < length; x++)
            Set(source, destinationIndex + x, sourceArray[sourceIndex + x]!);
    }

    /// <summary>
    ///     Copies elements from the FixedArrayPtr to the destination array.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read/write from.</param>
    /// <param name="destinationArray">The destination array to copy elements to.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyTo<TSource>(TSource source, Span<T> destinationArray, int length)
        where TSource : ICanReadWriteMemory => CopyTo(source, destinationArray, length, 0, 0);

    /// <summary>
    ///     Copies elements from the FixedArrayPtr to the destination array.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read/write from.</param>
    /// <param name="destinationArray">The destination array to copy elements to.</param>
    /// <param name="length">The number of elements to copy.</param>
    /// <param name="sourceIndex">The index in the FixedArrayPtr to start copying from. Default is 0.</param>
    /// <param name="destinationIndex">The index in the destination array to start copying to. Default is 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">One of the arguments was out of range.</exception>
    public void CopyTo<TSource>(TSource source, Span<T> destinationArray, int length, int sourceIndex,
        int destinationIndex) where TSource : ICanReadWriteMemory
    {
        if (length < 0 || sourceIndex < 0 || sourceIndex + length > Count || destinationIndex < 0 ||
            destinationIndex + length > destinationArray.Length)
            ThrowHelpers.ThrowArgumentOutOfRangeException(length, sourceIndex, destinationIndex,
                destinationArray.Length, Count);

        for (var x = 0; x < length; x++)
            destinationArray[destinationIndex + x] = Get(source, sourceIndex + x);
    }
}
