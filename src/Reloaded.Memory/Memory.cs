using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Enums;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Native.Unix;
using Reloaded.Memory.Native.Windows;
using Reloaded.Memory.Structs;
using Reloaded.Memory.Utilities;
#if NET5_0_OR_GREATER
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;
#endif

namespace Reloaded.Memory;

/// <summary>
///     Wrapper around memory read/write/allocate in current process. Supports Windows, Linux and OSX.
/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
public unsafe partial struct Memory : ICanReadWriteMemory, ICanAllocateMemory, ICanChangeMemoryProtection
{
    /// <summary>
    ///     Allows you to access the memory for the currently running process.
    /// </summary>
    public static readonly Memory Instance = new();

    #region ICanReadWriteMemory

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadRef<T>(nuint offset, ref T value) where T : unmanaged
        => value = Unsafe.ReadUnaligned<T>((void*)offset);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadWithMarshalling<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(PublicConstructors | NonPublicConstructors)]
#endif
        T>(nuint offset, [DisallowNull] ref T value)
    {
        value = Marshal.PtrToStructure<T>((nint)offset)!;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadRaw(nuint offset, Span<byte> value)
    {
        fixed (byte* result = value)
            Unsafe.CopyBlockUnaligned(result, (void*)offset, (uint)value.Length);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write<T>(nuint offset, in T item) where T : unmanaged => Unsafe.WriteUnaligned((void*)offset, item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteWithMarshalling<T>(nuint offset, [DisallowNull] in T item)
        => Marshal.StructureToPtr(item, (nint)offset, false);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteRaw(nuint offset, Span<byte> data)
    {
        fixed (byte* srcPtr = data)
            Unsafe.CopyBlockUnaligned((void*)offset, srcPtr, (uint)data.Length);
    }

    #endregion ICanReadWriteMemory

#pragma warning disable CA1416 // This API requires the operating system version to be checked

    #region ICanAllocateMemory

    /// <inheritdoc />
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public MemoryAllocation Allocate(nuint length)
    {
        if (Polyfills.IsWindows())
        {
            /*
                DO NOT USE Marshal.AllocHGlobal (GlobalAlloc)

                MSDN: It is best to avoid using VirtualProtect to change page protections on memory blocks allocated by GlobalAlloc,
                HeapAlloc, or LocalAlloc, because multiple memory blocks can exist on a single page. The heap manager assumes that all
                pages in the heap grant at least read and write access.
            */

            nuint returnAddress = Kernel32.VirtualAlloc
            (
                UIntPtr.Zero,
                length,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
                Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );

            if (returnAddress == 0)
                ThrowHelpers.ThrowMemoryAllocationExceptionWindows(length);

            return new MemoryAllocation(returnAddress, length);
        }

        if (Polyfills.IsLinux() || Polyfills.IsMacOS())
        {
            // 0x22 = Posix.MAP_PRIVATE | Posix.MAP_ANONYMOUS
            var flags = Polyfills.IsLinux() ? 0x22 : 0x1002;
            IntPtr result = Posix.mmap(UIntPtr.Zero, length, (int)MemoryProtection.ReadWriteExecute, flags, -1, 0);
            if (result == new IntPtr(-1))
                ThrowHelpers.ThrowMemoryAllocationExceptionPosix(length, (int)result);

            // ReSharper disable once RedundantCast
            return new MemoryAllocation((nuint)(nint)result, length);
        }

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return new MemoryAllocation();
    }

    /// <inheritdoc />
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public bool Free(MemoryAllocation allocation)
    {
        if (Polyfills.IsWindows())
        {
            return Kernel32.VirtualFree(allocation.Address, UIntPtr.Zero, Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
        }

        if (Polyfills.IsLinux() || Polyfills.IsMacOS())
        {
            var result = Posix.munmap(allocation.Address, allocation.Length);
            return result == 0;
        }

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return false;
    }

    #endregion ICanAllocateMemory

    #region ICanChangeMemoryProtection

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public nuint ChangeProtectionRaw(nuint memoryAddress, int size, nuint newProtection)
    {
        if (Polyfills.IsWindows())
        {
            var result = Kernel32.VirtualProtect(memoryAddress, (nuint)size, (Kernel32.MEM_PROTECTION)newProtection,
                out Kernel32.MEM_PROTECTION oldPermissions);
            if (!result)
                ThrowHelpers.ThrowMemoryPermissionExceptionWindows(memoryAddress, size, newProtection);

            return (nuint)oldPermissions;
        }

        if (Polyfills.IsLinux() || Polyfills.IsMacOS())
        {
            var result = Posix.mprotect(memoryAddress, (nuint)size, (UnixMemoryProtection)newProtection);
            if (result != 0)
                ThrowHelpers.ThrowMemoryPermissionExceptionPosix(memoryAddress, size, newProtection, result);

            return newProtection;
        }

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return UIntPtr.Zero;
    }

    #endregion ICanChangeMemoryProtection

#pragma warning restore CA1416 // This API requires the operating system version to be checked
}
