using System;
using Reloaded.Memory.Enums;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Native.Unix;
using Reloaded.Memory.Native.Windows;
using Reloaded.Memory.Structs;
using Reloaded.Memory.Tests.Utilities;
using Xunit;
using static Reloaded.Memory.Tests.Utilities.MemorySources;

namespace Reloaded.Memory.Tests.Tests.Memory;

/// <summary>
///     Tests related to changing memory protection.
/// </summary>
public class ChangeProtectionTests
{
    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ChangeProtection_Success(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0x100;
        MemoryAllocation allocation = source.AllocateMemory.Allocate(allocLength);

        // We don't have a reliable way to test Access Violations across platforms. Pretend it works if it doesn't throw.
        source.ChangeMemoryProtection.ChangeProtection(allocation.Address, allocLength, MemoryProtection.Read);
        source.ChangeMemoryProtection.ChangeProtection(allocation.Address, allocLength,
            MemoryProtection.ReadWriteExecute);
        source.ReadWriteMemory.Write(allocation.Address, 5);

        source.AllocateMemory.Free(allocation);
    }

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void SafeWriteAndRead_Success(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0x100;
        using DisposableMemoryAllocation<ICanAllocateMemory> allocation =
            source.AllocateMemory.AllocateDisposable(allocLength);
        source.ChangeMemoryProtection.ChangeProtection(allocation.Allocation.Address, allocLength,
            MemoryProtection.Read);

        // Prepare test data
        var dataToWrite = new byte[allocLength];
        new Random().NextBytes(dataToWrite);
        Span<byte> dataToWriteSpan = dataToWrite;

        // Test SafeWrite
        source.SafeWrite(allocation.Allocation.Address, dataToWriteSpan);

        // Test SafeRead
        var dataRead = new byte[allocLength];
        Span<byte> dataReadSpan = dataRead;
        source.SafeRead(allocation.Allocation.Address, dataReadSpan);

        // Verify that the written and read data are the same
        Assert.Equal(dataToWrite, dataRead);
    }

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ChangeProtection_OnFailure_ThrowsMemoryPermissionException(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        Assert.Throws<MemoryPermissionException>(() =>
        {
            source.ChangeMemoryProtection.ChangeProtection(unchecked((nuint)(-1)), 0x100,
                MemoryProtection.ReadWriteExecute);
        });
    }

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ChangeProtection_Disposable(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0x100;
        using DisposableMemoryAllocation<ICanAllocateMemory> allocation =
            source.AllocateMemory.AllocateDisposable(allocLength);

        // We don't have a reliable way to test Access Violations across platforms. Pretend it works if it doesn't throw.
        using DisposableMemoryProtection<ICanChangeMemoryProtection> disposable =
            source.ChangeMemoryProtection.ChangeProtectionDisposable(allocation.Allocation.Address, allocLength,
                MemoryProtection.Read);
    }

    // Mapping Tests
#pragma warning disable CA1416 // Validate platform compatibility
    [Theory]
    [InlineData(MemoryProtection.Read, UnixMemoryProtection.PROT_READ)]
    [InlineData(MemoryProtection.Write, UnixMemoryProtection.PROT_WRITE)]
    [InlineData(MemoryProtection.Execute, UnixMemoryProtection.PROT_EXEC)]
    public void ToUnixTest(MemoryProtection protection, UnixMemoryProtection expected)
    {
        var result = MemoryProtectionExtensions.ToUnix(protection);
        Assert.Equal((nuint)expected, result);
    }

    [Theory]
    [InlineData(MemoryProtection.Read, Kernel32.MEM_PROTECTION.PAGE_READONLY)]
    [InlineData(MemoryProtection.Write, Kernel32.MEM_PROTECTION.PAGE_READWRITE)]
    [InlineData(MemoryProtection.Execute, Kernel32.MEM_PROTECTION.PAGE_EXECUTE)]
    [InlineData(MemoryProtection.Read | MemoryProtection.Write, Kernel32.MEM_PROTECTION.PAGE_READWRITE)]
    [InlineData(MemoryProtection.Read | MemoryProtection.Execute, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READ)]
    [InlineData(MemoryProtection.Write | MemoryProtection.Execute, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE)]
    [InlineData(MemoryProtection.Read | MemoryProtection.Write | MemoryProtection.Execute,
        Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE)]
    public void ToWindowsTest(MemoryProtection protection, Kernel32.MEM_PROTECTION expected)
    {
        var result = MemoryProtectionExtensions.ToWindows(protection);
        Assert.Equal((nuint)expected, result);
    }
#pragma warning restore CA1416 // Validate platform compatibility
}
