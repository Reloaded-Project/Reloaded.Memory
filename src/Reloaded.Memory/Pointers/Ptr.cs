namespace Reloaded.Memory.Pointers;

/// <summary>
///     Blittable single level pointer type that you can use with generic types.
/// </summary>
/// <typeparam name="T">The item behind the blittable pointer.</typeparam>
/// <remarks>
///     This type was formerly called 'BlittablePointer' but was renamed to 'Ptr' for conciseness.
/// </remarks>
[PublicAPI]
public unsafe struct Ptr<T> where T : unmanaged
{
    /// <summary>
    ///     The pointer to the value.
    /// </summary>
    public T* Pointer { get; set; }

    /// <summary>
    ///     Creates a blittable pointer
    /// </summary>
    /// <param name="pointer">Pointer to wrap around.</param>
    public Ptr(T* pointer) => Pointer = pointer;

    /// <summary>
    ///     Converts this <see cref="Ptr{T}" /> to a value reference.
    /// </summary>
    public ref T AsRef() => ref Unsafe.AsRef<T>(Pointer); // Implicit Conversion

    /// <summary>.</summary>
    /// <param name="operand"></param>
    public static implicit operator Ptr<T>(T* operand) => new(operand);

    /// <summary>.</summary>
    /// <param name="operand"></param>
    public static implicit operator T*(Ptr<T> operand) => operand.Pointer;
}
