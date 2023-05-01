namespace Reloaded.Memory.Exceptions;

/// <summary>
///     Helper class for throwing exceptions.
/// </summary>
internal abstract class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowEnumNotSupportedException<T>()
        => throw new NotSupportedException($"Enum type {typeof(T).Name} is not supported.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowPlatformNotSupportedException()
        => throw new PlatformNotSupportedException("Operating System in use is not supported.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowReadExternalMemoryExceptionWindows(nuint memoryAddress, int structSize)
        => throw new MemoryException(
            $"ReadProcessMemory failed to read {structSize} bytes of memory from {memoryAddress}. Error: {Marshal.GetLastWin32Error()}");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowWriteExternalMemoryExceptionWindows(nuint memoryAddress, int structSize)
        => throw new MemoryException(
            $"WriteProcessMemory failed to write {structSize} bytes of memory to {memoryAddress}. Error: {Marshal.GetLastWin32Error()}");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowMemoryAllocationExceptionWindows(nuint length)
        => ThrowMemoryAllocationExceptionPosix(length, Marshal.GetLastWin32Error());

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowMemoryAllocationExceptionPosix(nuint length, int result)
        => throw new MemoryAllocationException(
            $"Failed to allocate memory {length} bytes, {result} errorno. GetLastError: {Marshal.GetLastWin32Error()}");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowMemoryPermissionExceptionWindows(nuint memoryAddress, int size, nuint newProtection)
        => ThrowMemoryPermissionExceptionPosix(memoryAddress, size, newProtection, Marshal.GetLastWin32Error());

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowMemoryPermissionExceptionPosix(nuint memoryAddress, int size, nuint newProtection,
        int result) => throw new MemoryPermissionException(
        $"Unable to change permissions for the following memory address {memoryAddress} of size {size} and permission {newProtection}. Error: {result}. SetLastError: {Marshal.GetLastWin32Error()}");
}
