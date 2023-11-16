namespace Reloaded.Memory.Exceptions;

/// <summary>
///     Helper class for throwing exceptions.
/// </summary>
internal abstract class ThrowHelpers
{
#if !NET7_0_OR_GREATER
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowEndOfFileException() => throw new EndOfStreamException();
#endif

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowOverflowException() => throw new OverflowException();

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException() => throw new ArgumentOutOfRangeException();

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(string paramName)
        => throw new ArgumentOutOfRangeException(paramName);

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowEnumNotSupportedException<T>()
        => throw new NotSupportedException($"Enum type {typeof(T).Name} is not supported.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowTypeNotSupportedException<T>()
        => throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowPlatformNotSupportedException()
        => throw new PlatformNotSupportedException("Operating System in use is not supported.");

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArchitectureNotSupportedException()
        => throw new PlatformNotSupportedException("Architecture is not supported. Only 32/64 bit platforms are supported.");

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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(int length, int sourceIndex, int destinationIndex,
        int sourceArrayLength, int fixedArrayPtrCount)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");

        if (sourceIndex < 0 || sourceIndex + length > sourceArrayLength)
            throw new ArgumentOutOfRangeException(nameof(sourceIndex), "Invalid source index.");

        if (destinationIndex < 0 || destinationIndex + length > fixedArrayPtrCount)
            throw new ArgumentOutOfRangeException(nameof(destinationIndex), "Invalid destination index.");
    }
}
