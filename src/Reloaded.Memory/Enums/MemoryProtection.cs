using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Native.Unix;
using Reloaded.Memory.Native.Windows;
using Reloaded.Memory.Utilities;
using static Reloaded.Memory.Native.Unix.UnixMemoryProtection;
using static Reloaded.Memory.Native.Windows.Kernel32.MEM_PROTECTION;

namespace Reloaded.Memory.Enums;

/// <summary>
///     Lists the various memory protection modes available.
/// </summary>
[Flags]
public enum MemoryProtection
{
    /// <summary>
    ///     Allows you to read the memory.
    /// </summary>
    Read = 1 << 0,

    /// <summary>
    ///     Allows you to write the memory.
    /// </summary>
    Write = 1 << 1,

    /// <summary>
    ///     Allows you to execute the memory.
    /// </summary>
    Execute = 1 << 2,

    /// <summary>
    ///     Allows you to read, write and execute
    /// </summary>
    ReadWriteExecute = Read | Write | Execute
}

/// <summary>
///     Extension methods for converting <see cref="MemoryProtection" /> to platform specific values.
/// </summary>
public static class MemoryProtectionExtensions
{
#pragma warning disable CA1416 // This API requires the operating system version to be checked
    /// <summary>
    ///     Converts a <see cref="MemoryProtection" /> to a platform specific value.
    /// </summary>
    /// <param name="protection">The protection to convert.</param>
    /// <returns>A platform specific value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint ToCurrentPlatform(this MemoryProtection protection)
    {
        // Check if is windows
        if (Polyfills.IsWindows())
            return ToWindows(protection);

        if (Polyfills.IsLinux() || Polyfills.IsMacOS())
            return ToUnix(protection);

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint ToUnix(MemoryProtection protection)
    {
        UnixMemoryProtection result = 0;
        if (protection.HasFlagFast(MemoryProtection.Read))
            result |= PROT_READ;
        if (protection.HasFlagFast(MemoryProtection.Write))
            result |= PROT_WRITE;
        if (protection.HasFlagFast(MemoryProtection.Execute))
            result |= PROT_EXEC;

        return (nuint)result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint ToWindows(MemoryProtection protection)
    {
        Kernel32.MEM_PROTECTION result = 0;

        if (protection.HasFlagFast(MemoryProtection.Read) && protection.HasFlagFast(MemoryProtection.Write) &&
            protection.HasFlagFast(MemoryProtection.Execute))
        {
            result = PAGE_EXECUTE_READWRITE;
        }
        else if (protection.HasFlagFast(MemoryProtection.Read) && protection.HasFlagFast(MemoryProtection.Write))
        {
            result = PAGE_READWRITE;
        }
        else if (protection.HasFlagFast(MemoryProtection.Read) && protection.HasFlagFast(MemoryProtection.Execute))
        {
            result = PAGE_EXECUTE_READ;
        }
        else if (protection.HasFlagFast(MemoryProtection.Write) && protection.HasFlagFast(MemoryProtection.Execute))
        {
            // There is no specific flag for Write + Execute, so we use PAGE_EXECUTE_READWRITE
            result = PAGE_EXECUTE_READWRITE;
        }
        else if (protection.HasFlagFast(MemoryProtection.Read))
        {
            result = PAGE_READONLY;
        }
        else if (protection.HasFlagFast(MemoryProtection.Write))
        {
            result = PAGE_READWRITE;
        }
        else if (protection.HasFlagFast(MemoryProtection.Execute))
        {
            result = PAGE_EXECUTE;
        }

        return (nuint)result;
    }
#pragma warning restore CA1416 // This API requires the operating system version to be checked
}
