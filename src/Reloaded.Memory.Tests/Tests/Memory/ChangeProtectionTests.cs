using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Memory.Enums;
using Reloaded.Memory.Memory.Interfaces;
using Reloaded.Memory.Memory.Structs;
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
        source.ChangeMemoryProtection.ChangeProtection(allocation.Address, allocLength, MemoryProtection.READ);
        source.ChangeMemoryProtection.ChangeProtection(allocation.Address, allocLength,
            MemoryProtection.READ_WRITE_EXECUTE);

        source.AllocateMemory.Free(allocation);
    }

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ChangeProtection_OnFailure_ThrowsMemoryPermissionException(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        Assert.Throws<MemoryPermissionException>(() =>
        {
            source.ChangeMemoryProtection.ChangeProtection(unchecked((nuint)(-1)), 0x100,
                MemoryProtection.READ_WRITE_EXECUTE);
        });
    }

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ChangeProtection_Disposable(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0x100;
        using var allocation = source.AllocateMemory.AllocateDisposable(allocLength);

        // We don't have a reliable way to test Access Violations across platforms. Pretend it works if it doesn't throw.
        using var disposable = source.ChangeMemoryProtection.ChangeProtectionDisposable(allocation.Allocation.Address, allocLength, MemoryProtection.READ);
    }
}
