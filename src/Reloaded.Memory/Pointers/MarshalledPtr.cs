using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Pointers;

/// <summary>
///     Single level pointer type that will read/write values with marshalling.
/// </summary>
/// <typeparam name="T">The item behind the pointer.</typeparam>
/// <remarks>
///     This is the 'managed' equivalent of <see cref="Ptr{T}" />.
///     Elements used with this struct must be fixed size.
/// </remarks>
[PublicAPI]
public unsafe struct MarshalledPtr<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
    T> : IEquatable<MarshalledPtr<T>> where T : new()
{
    /// <summary>
    ///     The pointer to the value.
    /// </summary>
    /// <remarks>Only use for pointers in same process.</remarks>
    public byte* Pointer;

    /// <summary>
    ///     Size of element after marshalling.
    /// </summary>
    public int ElementSize { get; private set; }

    /// <summary>
    ///     Creates a pointer to an element that needs marshalling (but is still fixed size).
    /// </summary>
    /// <param name="pointer">Pointer to wrap around.</param>
    public MarshalledPtr(byte* pointer)
    {
        Pointer = pointer;
        ElementSize = TypeInfo.MarshalledSizeOf<T>();
    }

    /// <summary>
    ///     Creates a pointer to an element that needs marshalling (but is still fixed size).
    /// </summary>
    /// <param name="pointer">Pointer to wrap around.</param>
    /// <param name="elementSize">Size of the element behind this pointer.</param>
    public MarshalledPtr(byte* pointer, int elementSize)
    {
        Pointer = pointer;
        ElementSize = elementSize;
    }

    /// <summary>
    ///     Gets the value at the address where the current pointer points to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <returns>The value at the pointer's address.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<TSource>(TSource source) where TSource : ICanReadWriteMemory
        => source.ReadWithMarshalling<T>((nuint)Pointer);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="value">The value at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get<TSource>(TSource source, out T? value) where TSource : ICanReadWriteMemory
        => source.ReadWithMarshallingOutParameter((nuint)Pointer, out value);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to write to.</param>
    /// <param name="value">The value to set at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<TSource>(TSource source, [DisallowNull] in T value) where TSource : ICanReadWriteMemory
        => source.WriteWithMarshalling((nuint)Pointer, value);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset from a given
    ///     <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="index">The index offset of the element.</param>
    /// <returns>The value at the pointer's address plus the index offset.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<TSource>(TSource source, int index) where TSource : ICanReadWriteMemory
        => source.ReadWithMarshalling<T>((nuint)(Pointer + index * ElementSize));

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset from a given
    ///     <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value at the pointer's address plus the index offset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get<TSource>(TSource source, int index, out T? value) where TSource : ICanReadWriteMemory
        => source.ReadWithMarshallingOutParameter((nuint)(Pointer + index * ElementSize), out value);

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
        => source.WriteWithMarshalling((nuint)(Pointer + index * ElementSize), value);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to.
    /// </summary>
    /// <returns>The value at the pointer's address.</returns>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get() => Marshal.PtrToStructure<T>((IntPtr)Pointer)!;

    /// <summary>
    ///     Gets the value at the address where the current pointer points to.
    /// </summary>
    /// <param name="value">The value at the pointer's address.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get(out T? value) => value = Marshal.PtrToStructure<T>((IntPtr)Pointer);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to.
    /// </summary>
    /// <param name="value">The value to set at the pointer's address.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set([DisallowNull] in T value) => Marshal.StructureToPtr<T>(value, (IntPtr)Pointer, false);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset.
    /// </summary>
    /// <param name="index">The index offset of the element.</param>
    /// <returns>The value at the pointer's address plus the index offset.</returns>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get(int index) => Marshal.PtrToStructure<T>((IntPtr)(Pointer + index * ElementSize))!;

    /// <summary>
    ///     Gets the value at the address where the current pointer points to plus the index offset.
    /// </summary>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value at the pointer's address plus the index offset.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get(int index, out T? value)
        => value = Marshal.PtrToStructure<T>((IntPtr)(Pointer + index * ElementSize))!;

    /// <summary>
    ///     Sets the value where the current pointer is pointing to plus the index offset.
    /// </summary>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value to set at the pointer's address plus the index offset.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int index, [DisallowNull] in T value)
        => Marshal.StructureToPtr<T>(value, (IntPtr)(Pointer + index * ElementSize), false);

    /// <summary>
    ///     Compares two <see cref="MarshalledPtr{T}" /> instances for equality.
    /// </summary>
    /// <param name="left">The left <see cref="MarshalledPtr{T}" /> to compare.</param>
    /// <param name="right">The right <see cref="MarshalledPtr{T}" /> to compare.</param>
    /// <returns>True if the pointers are equal, false otherwise.</returns>
    public static bool operator ==(MarshalledPtr<T> left, MarshalledPtr<T> right) => left.Pointer == right.Pointer;

    /// <summary>
    ///     Compares two <see cref="MarshalledPtr{T}" /> instances for inequality.
    /// </summary>
    /// <param name="left">The left <see cref="MarshalledPtr{T}" /> to compare.</param>
    /// <param name="right">The right <see cref="MarshalledPtr{T}" /> to compare.</param>
    /// <returns>True if the pointers are not equal, false otherwise.</returns>
    public static bool operator !=(MarshalledPtr<T> left, MarshalledPtr<T> right) => left.Pointer != right.Pointer;

    /// <summary>
    ///     Adds an integer offset to a <see cref="MarshalledPtr{T}" />.
    /// </summary>
    /// <param name="pointer">The <see cref="MarshalledPtr{T}" /> to add the offset to.</param>
    /// <param name="offset">The integer offset to add.</param>
    /// <returns>A new <see cref="MarshalledPtr{T}" /> with the updated address.</returns>
    public static MarshalledPtr<T> operator +(MarshalledPtr<T> pointer, int offset)
        => new(pointer.Pointer + offset * pointer.ElementSize, pointer.ElementSize);

    /// <summary>
    ///     Subtracts an integer offset from a <see cref="MarshalledPtr{T}" />.
    /// </summary>
    /// <param name="pointer">The <see cref="MarshalledPtr{T}" /> to subtract the offset from.</param>
    /// <param name="offset">The integer offset to subtract.</param>
    /// <returns>A new <see cref="MarshalledPtr{T}" /> with the updated address.</returns>
    public static MarshalledPtr<T> operator -(MarshalledPtr<T> pointer, int offset)
        => new(pointer.Pointer - offset * pointer.ElementSize, pointer.ElementSize);

    /// <summary>
    ///     Increments the address of the <see cref="Ptr{T}" /> by the size of <typeparamref name="T" />.
    /// </summary>
    /// <param name="pointer">The <see cref="Ptr{T}" /> to increment.</param>
    /// <returns>A new <see cref="Ptr{T}" /> with the incremented address.</returns>
    public static MarshalledPtr<T> operator ++(MarshalledPtr<T> pointer) => new(pointer.Pointer + pointer.ElementSize);

    /// <summary>
    ///     Decrements the address of the <see cref="Ptr{T}" /> by the size of <typeparamref name="T" />.
    /// </summary>
    /// <param name="pointer">The <see cref="Ptr{T}" /> to decrement.</param>
    /// <returns>A new <see cref="Ptr{T}" /> with the decremented address.</returns>
    public static MarshalledPtr<T> operator --(MarshalledPtr<T> pointer) => new(pointer.Pointer - pointer.ElementSize);

    /// <summary>
    ///     Determines if the <see cref="MarshalledPtr{T}" /> is considered "true" in a boolean context.
    /// </summary>
    /// <param name="operand">The <see cref="MarshalledPtr{T}" /> to evaluate.</param>
    /// <returns>True if the underlying pointer is not null, false otherwise.</returns>
    public static bool operator true(MarshalledPtr<T> operand) => operand.Pointer != null;

    /// <summary>
    ///     Determines if the <see cref="MarshalledPtr{T}" /> is considered "false" in a boolean context.
    /// </summary>
    /// <param name="operand">The <see cref="MarshalledPtr{T}" /> to evaluate.</param>
    /// <returns>True if the underlying pointer is null, false otherwise.</returns>
    public static bool operator false(MarshalledPtr<T> operand) => operand.Pointer == null;

    /// <summary>
    ///     Checks for the equality of the current <see cref="MarshalledPtr{T}" /> with another <see cref="MarshalledPtr{T}" />
    ///     .
    /// </summary>
    /// <param name="other">The other <see cref="MarshalledPtr{T}" /> to compare with.</param>
    /// <returns>True if the pointers are equal and have the same element size, false otherwise.</returns>
    public bool Equals(MarshalledPtr<T> other) => Pointer == other.Pointer && ElementSize == other.ElementSize;

    /// <summary>
    ///     Checks for the equality of the current <see cref="MarshalledPtr{T}" /> with another object.
    /// </summary>
    /// <param name="obj">The other object to compare with.</param>
    /// <returns>
    ///     True if the object is a <see cref="MarshalledPtr{T}" /> and the pointers are equal and have the same element
    ///     size, false otherwise.
    /// </returns>
    public override bool Equals(object? obj) => obj is MarshalledPtr<T> other && Equals(other);

    /// <summary>
    ///     Gets the hash code of the current <see cref="MarshalledPtr{T}" />.
    /// </summary>
    /// <returns>The hash code of the current <see cref="MarshalledPtr{T}" />.</returns>
    public override int GetHashCode() => ((IntPtr)Pointer).GetHashCode() ^ ElementSize;

    /// <inheritdoc />
    public override string ToString() => $"MarshalledPtr<{typeof(T).Name}> ({(ulong)Pointer:X})";
}
