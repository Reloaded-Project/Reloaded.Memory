using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Structs;
using Reloaded.Memory.Tests.Utilities;
using Xunit;
using static Reloaded.Memory.Tests.Utilities.MemorySources;

namespace Reloaded.Memory.Tests.Tests.Memory;

/// <summary>
///     Tests related to allocating memory.
/// </summary>
public class MemoryAllocationTests
{
    /// <summary>
    ///     Tests memory allocation by checking if the result points to valid memory (nonzero).
    /// </summary>
    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void AllocateMemory_ValidAllocation_Success(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0xFFFF;

        MemoryAllocation alloc = source.AllocateMemory.Allocate(allocLength);

        alloc.Length.Should().Be((nuint)allocLength);
        alloc.Address.Should().NotBe((nuint)0);

        source.AllocateMemory.Free(alloc);
    }

    /// <summary>
    ///     Tests memory allocation by checking if the result points to valid memory (nonzero).
    /// </summary>
    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void AllocateMemory_Disposable(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0xFFFF;

        using DisposableMemoryAllocation<ICanAllocateMemory> alloc =
            source.AllocateMemory.AllocateDisposable(allocLength);
        alloc.Allocation.Length.Should().Be((nuint)allocLength);

        // You cannot directly verify the memory was released since it's managed by the operating system.
        // The 'using' statement ensures that the Dispose method is called, which releases the memory.
    }
}
