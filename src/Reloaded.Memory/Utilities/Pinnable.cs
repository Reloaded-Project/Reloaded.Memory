namespace Reloaded.Memory.Utilities;

/// <summary>
///     Allows you to pin a native unmanaged object in a static location in memory, to be
///     later accessible from native code.
/// </summary>
/// <typeparam name="T">The type of element being pinned in native memory.</typeparam>
[PublicAPI]
public unsafe class Pinnable<T> : IDisposable where T : unmanaged
{
    /// <summary>
    ///     The value pointed to by the <see cref="Pointer" />.
    ///     If the class was instantiated using an array, this is the first element of the array.
    /// </summary>
    public ref T Value => ref Unsafe.AsRef<T>(Pointer);

    /// <summary>
    ///     Pointer to the native value in question.
    ///     If the class was instantiated using an array, this is the pointer to the first element of the array.
    /// </summary>
    public T* Pointer { get; private set; }

#if NET5_0_OR_GREATER
    // Array in pinned object heap.
    private T[] _pohReference = null!;
#else
    // Handle keeping the object pinned.
    private GCHandle _handle;
#endif

    private bool _disposed;

    /* Constructor/Destructor */

    // Note: GCHandle.Alloc causes boxing(due to conversion to object), meaning our item is stored on the heap.
    // This means that for value types, we do not need to store them explicitly.

    /// <summary>
    ///     Pins an array of values to the heap.
    /// </summary>
    /// <param name="value">
    ///     The values to be pinned on the heap.
    ///     Depending on runtime used, these may be copied; so please use <see cref="Pointer" /> property.
    ///     Do not use original array once passed to this function.
    /// </param>
    public Pinnable(T[] value)
    {
#if NET5_0_OR_GREATER
        _pohReference = GC.AllocateUninitializedArray<T>(value.Length, true);
        Array.Copy(value, _pohReference, value.Length);
        Pointer = (T*)Unsafe.AsPointer(ref _pohReference[0]);
#else
        _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        Pointer = (T*)_handle.AddrOfPinnedObject();
#endif
    }

    /// <summary>
    ///     Pins a value to the heap.
    /// </summary>
    /// <param name="value">The value to be pinned on the heap.</param>
    public Pinnable(in T value) => InitFromReference(value);

    private void InitFromReference(in T value)
    {
#if NET5_0_OR_GREATER
        _pohReference = GC.AllocateUninitializedArray<T>(1, true);
        Pointer = (T*)Unsafe.AsPointer(ref _pohReference[0]);
        *Pointer = value;
#else
        _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        Pointer = (T*)_handle.AddrOfPinnedObject();
#endif
    }

    /// <summary>
    ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by
    ///     garbage collection.
    /// </summary>
    ~Pinnable() => Dispose();

    /// <inheritdoc />
    public void Dispose()
    {
        // Add disposed check
        if (_disposed)
            return;

        _disposed = true;

#if NET5_0_OR_GREATER
        _pohReference = null!;
#else
        if (_handle.IsAllocated)
            _handle.Free();
#endif

        Pointer = (T*)0;
        GC.SuppressFinalize(this);
    }
}
