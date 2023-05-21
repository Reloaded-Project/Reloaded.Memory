using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Structs;

/// <summary>
///     A memory allocation with a disposable interface.
/// </summary>
/// <typeparam name="TProtector">Type of allocator used to create this allocation.</typeparam>
public readonly struct DisposableMemoryProtection<TProtector> : IDisposable where TProtector : ICanChangeMemoryProtection
{
    /// <summary>
    ///     Memory address where protection is to be disabled.
    /// </summary>
    public nuint MemoryAddress { get; init; }

    /// <summary>
    ///     The original protection set.
    /// </summary>
    public nuint OriginalProtection { get; init; }

    /// <summary>
    ///     The allocator which created this instance.
    /// </summary>
    public TProtector Protector { get; init; }

    /// <summary>
    ///     Size of the memory.
    /// </summary>
    public int Size { get; init; }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => Protector.ChangeProtectionRaw(MemoryAddress, Size, OriginalProtection);
}
