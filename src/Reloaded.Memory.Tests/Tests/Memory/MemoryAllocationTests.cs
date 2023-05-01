﻿using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Reloaded.Memory.Memory.Interfaces;
using Reloaded.Memory.Memory.Structs;
using Reloaded.Memory.Tests.Utilities;
using Reloaded.Memory.Utility;
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

        alloc.Length.Should().Be((UIntPtr)allocLength);
        alloc.Address.Should().NotBe((UIntPtr)0);

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

        using var alloc = source.AllocateMemory.AllocateDisposable(allocLength);
        alloc.Allocation.Length.Should().Be((nuint)allocLength);

        // You cannot directly verify the memory was released since it's managed by the operating system.
        // The 'using' statement ensures that the Dispose method is called, which releases the memory.
    }
}