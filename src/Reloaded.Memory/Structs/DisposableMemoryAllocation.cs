using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Structs;

/// <summary>
///     A memory allocation with a disposable interface.
/// </summary>
/// <typeparam name="TAllocator">Type of allocator used to create this allocation.</typeparam>
public readonly struct DisposableMemoryAllocation<TAllocator> : IDisposable where TAllocator : ICanAllocateMemory
{
    /// <summary>
    ///     The memory allocation that's disposable.
    /// </summary>
    public MemoryAllocation Allocation { get; init; }

    /// <summary>
    ///     The allocator which created this instance.
    /// </summary>
    public TAllocator Allocator { get; init; }

    /// <inheritdoc />
    public void Dispose() => Allocator.Free(Allocation);
}
