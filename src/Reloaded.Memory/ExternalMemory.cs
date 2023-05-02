﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Native.Unix;
using Reloaded.Memory.Native.Windows;
using Reloaded.Memory.Structs;
using Reloaded.Memory.Utility;

namespace Reloaded.Memory;

/// <summary>
///     Provides access to memory of another process on a Windows machine.
/// </summary>
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
#endif
// ReSharper disable once PartialTypeWithSinglePart
[SuppressMessage("Interoperability", "CA1416: Validate platform compatibility")] // false report
public unsafe partial struct ExternalMemory : ICanReadWriteMemory, ICanAllocateMemory, ICanChangeMemoryProtection
{
    /// <summary>
    ///     Contains the handle of the process used to read memory
    ///     from and write memory to external process.
    /// </summary>
    private readonly nint _processHandle;

    /*
         --------------
         Constructor(s)
         --------------
    */

    /// <summary>
    ///     Creates an instance of the <see cref="ExternalMemory" /> class used to read from an
    ///     external process with a specified handle.
    /// </summary>
    /// <param name="processHandle">
    ///     Handle of the process to read/write memory from.
    ///     On Linux, please insert Process ID here.
    /// </param>
    public ExternalMemory(nint processHandle) => _processHandle = processHandle;

    /// <summary>
    ///     Creates an instance of the <see cref="ExternalMemory" /> class used to read from an
    ///     external process with a specified handle.
    /// </summary>
    /// <param name="process">The individual process to read/write memory from.</param>
    public ExternalMemory(Process process) : this()
    {
        if (Polyfills.IsWindows())
            _processHandle = process.Handle;
        else if (Polyfills.IsLinux())
            _processHandle = process.Id;
        else
            ThrowHelpers.ThrowPlatformNotSupportedException();
    }

    #region ICanReadWriteMemory

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadRef<T>(nuint offset, ref T value) where T : unmanaged
    {
        void* bufferPtr = Unsafe.AsPointer(ref value);
        bool succeeded = ReadProcessMemory(offset, bufferPtr, (nuint)sizeof(T));
        if (!succeeded)
            ThrowHelpers.ThrowReadExternalMemoryExceptionWindows(offset, sizeof(T));
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadWithMarshalling<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(nuint offset, [DisallowNull] ref T value)
    {
        int structSize = Marshal.SizeOf(value);
        if (structSize <= 1024)
        {
            // Hot path.
            byte* bufferPtr = stackalloc byte[structSize];
            ReadWithMarshallingImpl(offset, value, bufferPtr, structSize);
        }
        else
        {
            // Cold path.
            using var alloc = new Memory().AllocateDisposable((UIntPtr)structSize);
            ReadWithMarshallingImpl(offset, value, (byte*)alloc.Allocation.Address, structSize);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReadWithMarshallingImpl<T>(nuint offset, [DisallowNull] T value, byte* bufferPtr, int structSize)
    {
        bool succeeded = ReadProcessMemory(offset, bufferPtr, (nuint)structSize);
        if (!succeeded)
            ThrowHelpers.ThrowReadExternalMemoryExceptionWindows(offset, structSize);

        Marshal.PtrToStructure((nint)bufferPtr, value);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadRaw(nuint offset, Span<byte> value)
    {
        fixed (byte* bufferPtr = value)
        {
            bool succeeded = ReadProcessMemory(offset, bufferPtr, (nuint)value.Length);
            if (!succeeded)
                ThrowHelpers.ThrowReadExternalMemoryExceptionWindows(offset, value.Length);
        }
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write<T>(nuint offset, in T item) where T : unmanaged
    {
        ref T itemRef = ref Unsafe.AsRef(in item);
        void* itemPtr = Unsafe.AsPointer(ref itemRef);

        bool succeeded = WriteProcessMemory(offset, (byte*)itemPtr, (nuint)sizeof(T));
        if (!succeeded)
            ThrowHelpers.ThrowWriteExternalMemoryExceptionWindows(offset, sizeof(T));
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteWithMarshalling<T>(nuint offset, [DisallowNull] in T item)
    {
        int structSize = Marshal.SizeOf(item);
        if (structSize <= 1024)
        {
            // Hot Path
            byte* bufferPtr = stackalloc byte[structSize];
            Marshal.StructureToPtr<T>(item, (nint)bufferPtr, false);
            WriteWithMarshallingImpl((byte*)offset, bufferPtr, structSize);
        }
        else
        {
            // Cold Path
            using var alloc = new Memory().AllocateDisposable((UIntPtr)structSize);
            Marshal.StructureToPtr<T>(item, (nint)alloc.Allocation.Address, false);
            WriteWithMarshallingImpl((byte*)offset, (byte*)alloc.Allocation.Address, structSize);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteWithMarshallingImpl(byte* offset, byte* itemPtr, int structSize)
    {
        bool succeeded = WriteProcessMemory((nuint)offset, itemPtr, (nuint)structSize);
        if (!succeeded)
            ThrowHelpers.ThrowWriteExternalMemoryExceptionWindows((nuint)offset, structSize);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteRaw(nuint offset, Span<byte> data)
    {
        fixed (byte* bytePtr = data)
        {
            bool succeeded = WriteProcessMemory(offset, bytePtr, (nuint)data.Length);
            if (!succeeded)
                ThrowHelpers.ThrowWriteExternalMemoryExceptionWindows(offset, data.Length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WriteProcessMemory(nuint offset, byte* bytePtr, nuint length)
    {
        if (Polyfills.IsWindows())
            return Kernel32.WriteProcessMemory(_processHandle, offset, (nuint)bytePtr, length, out _);

        if (Polyfills.IsLinux())
            return Posix.process_vm_writev_k32(_processHandle, offset, (nuint)bytePtr, length);

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ReadProcessMemory(nuint location, void* buffer, nuint numBytes)
    {
        if (Polyfills.IsWindows())
            return Kernel32.ReadProcessMemory(_processHandle, location, (nuint)buffer, numBytes, out _);

        if (Polyfills.IsLinux())
            return Posix.process_vm_readv_k32(_processHandle, location, (nuint)buffer, numBytes);

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return false;
    }

    #endregion ICanReadWriteMemory

    #region ICanAllocateMemory

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MemoryAllocation Allocate(nuint length)
    {
        // Call VirtualAllocEx to allocate memory of fixed chosen size.
        if (Polyfills.IsWindows())
        {
            nuint returnAddress = Kernel32.VirtualAllocEx
            (
                _processHandle,
                UIntPtr.Zero,
                length,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT | Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE,
                Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );

            if (returnAddress == 0)
                ThrowHelpers.ThrowMemoryAllocationExceptionWindows(length);

            return new MemoryAllocation(returnAddress, length);
        }

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return default;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Free(MemoryAllocation allocation) => Kernel32.VirtualFreeEx(_processHandle, allocation.Address,
        UIntPtr.Zero,
        Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);

    #endregion ICanAllocateMemory

    #region ICanChangeProtection

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint ChangeProtectionRaw(nuint memoryAddress, int size, nuint newProtection)
    {
        bool result = Kernel32.VirtualProtectEx(_processHandle, memoryAddress, (nuint)size,
            (Kernel32.MEM_PROTECTION)newProtection, out Kernel32.MEM_PROTECTION oldPermissions);

        if (!result)
            ThrowHelpers.ThrowMemoryPermissionExceptionWindows(memoryAddress, size, newProtection);

        return (nuint)oldPermissions;
    }

    #endregion ICanChangeProtection
}