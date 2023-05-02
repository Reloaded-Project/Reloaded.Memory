using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     A simple interface that allows the user to allocate/free memory.
///     This is an extension of <see cref="ICanReadWriteMemory" />.
/// </summary>
public interface ICanAllocateMemory
{
    /// <summary>
    ///     Allocates fixed size of memory inside the target memory source.
    ///     Returns the address of newly allocated memory.
    /// </summary>
    /// <param name="length">Amount of bytes to be allocated.</param>
    /// <exception cref="NotImplementedException">Thrown if a deriving class does not implement this function.</exception>
    /// <exception cref="MemoryAllocationException">Failed to allocate memory.</exception>
    /// <exception cref="MemoryPermissionException">
    ///     On Unix (Linux and OSX) if failed to change permission for newly allocated
    ///     memory.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">Unsupported on this platform.</exception>
    /// <returns>Address to the newly allocated memory.</returns>
    MemoryAllocation Allocate(nuint length);

    /// <summary>
    ///     Frees memory previously allocated with <see cref="Allocate" />.
    /// </summary>
    /// <param name="allocation">The allocation to free.</param>
    /// <exception cref="NotImplementedException">Thrown if a deriving class does not implement this function.</exception>
    /// <exception cref="PlatformNotSupportedException">Unsupported on this platform.</exception>
    /// <returns>True if the operation is successful.</returns>
    bool Free(MemoryAllocation allocation);
}

/// <summary>
///     Extension methods for implementers of <see cref="ICanAllocateMemory" />.
/// </summary>
public static class CanAllocateMemoryExtensions
{
    /// <summary>
    ///     Allocates a disposable memory allocation. When the allocation is disposed, the memory is freed.
    ///     Use with `using` statement.
    /// </summary>
    /// <param name="allocator">Allocator used to allocate/free the memory.</param>
    /// <param name="numBytes">Number of bytes.</param>
    /// <typeparam name="TAllocator">Allocator type used</typeparam>
    /// <returns>
    ///     Disposable memory allocation.
    ///     When the allocation is disposed, the memory is freed.
    ///     Use with `using` statement.
    /// </returns>
    public static DisposableMemoryAllocation<TAllocator>
        AllocateDisposable<TAllocator>(this TAllocator allocator, nuint numBytes) where TAllocator : ICanAllocateMemory
        => new() { Allocation = allocator.Allocate(numBytes), Allocator = allocator };
}
