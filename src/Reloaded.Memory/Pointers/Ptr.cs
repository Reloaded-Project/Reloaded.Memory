using Reloaded.Memory.Interfaces;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Reloaded.Memory.Pointers;

/// <summary>
///     A blittable single level pointer type that you can use with generic types.
/// </summary>
/// <typeparam name="T">The item behind the blittable pointer.</typeparam>
/// <remarks>
///     This type was formerly called 'BlittablePointer' but was renamed to 'Ptr' for conciseness.
/// </remarks>
[PublicAPI]
public unsafe struct Ptr<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
    T> : IEquatable<Ptr<T>> where T : unmanaged
{
    /// <summary>
    ///     The pointer to the value.
    ///     For reference only; only use for pointers in RAM (based on <see cref="Memory{T}" />), otherwise use methods!
    /// </summary>
    /// <remarks>Only use for pointers in same process.</remarks>
    public T* Pointer;

    /// <summary>
    ///     Creates a blittable pointer
    /// </summary>
    /// <param name="pointer">Pointer to wrap around.</param>
    public Ptr(T* pointer) => Pointer = pointer;

    /// <summary>
    ///     Converts this <see cref="Ptr{T}" /> to a value reference.
    ///     Only use for pointers in RAM (based on <see cref="Memory{T}" />), otherwise use methods!
    /// </summary>
    /// <remarks>
    ///     Only use for pointers within same process.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T AsRef() => ref Unsafe.AsRef<T>(Pointer);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <returns>The value at the pointer's address.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<TSource>(TSource source) where TSource : ICanReadWriteMemory => source.Read<T>((nuint)Pointer);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to read from.</param>
    /// <param name="value">The value at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get<TSource>(TSource source, out T value) where TSource : ICanReadWriteMemory
        => source.Read((nuint)Pointer, out value);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to from a given <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to write to.</param>
    /// <param name="value">The value to set at the pointer's address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<TSource>(TSource source, in T value) where TSource : ICanReadWriteMemory
        => source.Write((nuint)Pointer, value);

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
        => source.Read<T>((nuint)(Pointer + index));

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
        => source.Read((nuint)(Pointer + index), out value);

    /// <summary>
    ///     Sets the value where the current pointer is pointing to plus the index offset from a given
    ///     <typeparamref name="TSource" />.
    /// </summary>
    /// <typeparam name="TSource">Source implementing the <see cref="ICanReadWriteMemory" /> interface.</typeparam>
    /// <param name="source">The memory source to write to.</param>
    /// <param name="index">The index offset of the element.</param>
    /// <param name="value">The value to set at the pointer's address plus the index offset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<TSource>(TSource source, int index, in T value) where TSource : ICanReadWriteMemory
        => source.Write((nuint)(Pointer + index), value);

    /// <summary>
    ///     Gets the value at the address where the current pointer points to.
    /// </summary>
    /// <returns>The value at the pointer's address.</returns>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get() => *Pointer;

    /// <summary>
    ///     Gets the value at the address where the current pointer points to.
    /// </summary>
    /// <param name="value">The value at the pointer's address.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Get(out T value) => value = *Pointer;

    /// <summary>
    ///     Sets the value where the current pointer is pointing to.
    /// </summary>
    /// <param name="value">The value to set at the pointer's address.</param>
    /// <remarks>
    ///     This reads from computer's RAM and is just for convenience. Please use overload to read from an
    ///     <see cref="ICanReadWriteMemory" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(in T value) => *Pointer = value;

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
    public T Get(int index) => Pointer[index];

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
    public void Get(int index, out T value) => value = Pointer[index];

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
    public void Set(int index, in T value) => Pointer[index] = value;

    // Overridable operators
    /// <summary>
    ///     Compares two <see cref="Ptr{T}" /> instances for equality.
    /// </summary>
    /// <param name="left">The left <see cref="Ptr{T}" /> to compare.</param>
    /// <param name="right">The right <see cref="Ptr{T}" /> to compare.</param>
    /// <returns>True if the pointers are equal, false otherwise.</returns>
    public static bool operator ==(Ptr<T> left, Ptr<T> right) => left.Pointer == right.Pointer;

    /// <summary>
    ///     Compares two <see cref="Ptr{T}" /> instances for inequality.
    /// </summary>
    /// <param name="left">The left <see cref="Ptr{T}" /> to compare.</param>
    /// <param name="right">The right <see cref="Ptr{T}" /> to compare.</param>
    /// <returns>True if the pointers are not equal, false otherwise.</returns>
    public static bool operator !=(Ptr<T> left, Ptr<T> right) => left.Pointer != right.Pointer;

    /// <summary>
    ///     Adds an integer offset to a <see cref="Ptr{T}" />.
    /// </summary>
    /// <param name="pointer">The <see cref="Ptr{T}" /> to add the offset to.</param>
    /// <param name="offset">The integer offset to add.</param>
    /// <returns>A new <see cref="Ptr{T}" /> with the updated address.</returns>
    public static Ptr<T> operator +(Ptr<T> pointer, int offset) => new(pointer.Pointer + offset);

    /// <summary>
    ///     Subtracts an integer offset from a <see cref="Ptr{T}" />.
    /// </summary>
    /// <param name="pointer">The <see cref="Ptr{T}" /> to subtract the offset from.</param>
    /// <param name="offset">The integer offset to subtract.</param>
    /// <returns>A new <see cref="Ptr{T}" /> with the updated address.</returns>
    public static Ptr<T> operator -(Ptr<T> pointer, int offset) => new(pointer.Pointer - offset);

    /// <summary>
    ///     Increments the address of the <see cref="Ptr{T}" /> by the size of <typeparamref name="T" />.
    /// </summary>
    /// <param name="pointer">The <see cref="Ptr{T}" /> to increment.</param>
    /// <returns>A new <see cref="Ptr{T}" /> with the incremented address.</returns>
    public static Ptr<T> operator ++(Ptr<T> pointer) => new(pointer.Pointer + 1);

    /// <summary>
    ///     Decrements the address of the <see cref="Ptr{T}" /> by the size of <typeparamref name="T" />.
    /// </summary>
    /// <param name="pointer">The <see cref="Ptr{T}" /> to decrement.</param>
    /// <returns>A new <see cref="Ptr{T}" /> with the decremented address.</returns>
    public static Ptr<T> operator --(Ptr<T> pointer) => new(pointer.Pointer - 1);

    /// <summary>
    ///     Determines if the <see cref="Ptr{T}" /> is considered "true" in a boolean context.
    /// </summary>
    /// <param name="operand">The <see cref="Ptr{T}" /> to evaluate.</param>
    /// <returns>True if the underlying pointer is not null, false otherwise.</returns>
    public static bool operator true(Ptr<T> operand) => operand.Pointer != null;

    /// <summary>
    ///     Determines if the <see cref="Ptr{T}" /> is considered "false" in a boolean context.
    /// </summary>
    /// <param name="operand">The <see cref="Ptr{T}" /> to evaluate.</param>
    /// <returns>True if the underlying pointer is null, false otherwise.</returns>
    public static bool operator false(Ptr<T> operand) => operand.Pointer == null;

    /// <summary>
    ///     Implicitly converts a <see cref="Ptr{T}" /> to a pointer of type T.
    /// </summary>
    /// <param name="operand">The <see cref="Ptr{T}" /> to convert.</param>
    public static implicit operator Ptr<T>(T* operand) => new(operand);

    /// <summary>
    ///     Implicitly converts a pointer of type T to a <see cref="Ptr{T}" />.
    /// </summary>
    /// <param name="operand">The pointer of type T to convert.</param>
    public static implicit operator T*(Ptr<T> operand) => operand.Pointer;

    /// <inheritdoc />
    public override int GetHashCode() => (int)Pointer;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Ptr<T> other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Ptr<T> other) => this == other;

    /// <inheritdoc />
    public override string ToString() => $"Ptr<{typeof(T).Name}> ({(ulong)Pointer:X})";
}
