using Reloaded.Memory.Enums;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Native.Unix;
using Reloaded.Memory.Native.Windows;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     A simple interface that allows the user to change memory permissions on an arbitrary platform.
///     This is an extension of <see cref="ICanReadWriteMemory" />.
/// </summary>
public interface ICanChangeMemoryProtection
{
    /// <summary>
    ///     Changes the page permissions, setting them to a raw, platform specific value.
    /// </summary>
    /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
    /// <param name="size">The region size for which to change permissions for.</param>
    /// <param name="newProtection">
    ///     The new permissions to set; platform specific, e.g.
    ///     <see cref="Kernel32.MEM_PROTECTION" />.
    /// </param>
    /// <exception cref="NotImplementedException">Thrown if a deriving class does not implement this function.</exception>
    /// <exception cref="MemoryPermissionException">Failed to change permissions for the following memory address and size.</exception>
    /// <returns>The old page permissions.</returns>
    /// <remarks>
    ///     This function is intended for advanced users only.
    ///     <paramref name="newProtection" /> and the returned value is:<br />
    ///     - <see cref="UnixMemoryProtection" /> for Linux and OSX and<br />
    ///     - <see cref="Kernel32.MEM_PROTECTION" /> for Windows.<br />
    /// </remarks>
    nuint ChangeProtectionRaw(nuint memoryAddress, int size, nuint newProtection);
}

/// <summary>
///     Extension methods for <see cref="ICanChangeMemoryProtection" />.
/// </summary>
public static class CanChangeMemoryProtectionExtensions
{
    /// <summary>
    ///     Changes the page permissions for a specified combination of address and length.
    /// </summary>
    /// <typeparam name="T">Instance of <see cref="ICanChangeMemoryProtection" />.</typeparam>
    /// <param name="item">The item whose permissions are being changed.</param>
    /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
    /// <param name="size">The region size for which to change permissions for.</param>
    /// <param name="newProtection">The new permissions to set.</param>
    /// <exception cref="NotImplementedException">Thrown if a deriving class does not implement this function.</exception>
    /// <exception cref="MemoryPermissionException">Failed to change permissions for the following memory address and size.</exception>
    /// <returns>
    ///     The old permissions represented in a raw value you can feed to
    ///     <see cref="ICanChangeMemoryProtection.ChangeProtectionRaw" />.
    ///     Note that on some platforms this is unsupported, and will return requested permissions back instead.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint ChangeProtection<T>(this T item, nuint memoryAddress, int size, MemoryProtection newProtection)
        where T : ICanChangeMemoryProtection
        => item.ChangeProtectionRaw(memoryAddress, size, newProtection.ToCurrentPlatform());

    /// <summary>
    ///     Changes the page permissions for a specified combination of address and length.
    ///     This item can be disposed, use with `using` statement.
    /// </summary>
    /// <typeparam name="T">Instance of <see cref="ICanChangeMemoryProtection" />.</typeparam>
    /// <param name="item">The item whose permissions are being changed.</param>
    /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
    /// <param name="size">The region size for which to change permissions for.</param>
    /// <param name="newProtection">The new permissions to set.</param>
    /// <exception cref="NotImplementedException">Thrown if a deriving class does not implement this function.</exception>
    /// <exception cref="MemoryPermissionException">Failed to change permissions for the following memory address and size.</exception>
    /// <returns>
    ///     Disposable item which can be used to undo the changes.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DisposableMemoryProtection<T> ChangeProtectionDisposable<T>(this T item, nuint memoryAddress,
        int size, MemoryProtection newProtection)
        where T : ICanChangeMemoryProtection
    {
        nuint original = item.ChangeProtectionRaw(memoryAddress, size, newProtection.ToCurrentPlatform());
        return new DisposableMemoryProtection<T>
        {
            MemoryAddress = memoryAddress, OriginalProtection = original, Protector = item, Size = size
        };
    }

    /// <summary>
    ///     Writes data to the specified memory address, temporarily changing the memory permissions to read/write/execute.
    /// </summary>
    /// <param name="item">
    ///     Item inheriting from both <see cref="ICanChangeMemoryProtection" /> and
    ///     <see cref="ICanReadWriteMemory" />.
    /// </param>
    /// <param name="memoryAddress">The memory address to write to.</param>
    /// <param name="data">Data to write to the address.</param>
    /// <typeparam name="TMemory">
    ///     Item inheriting from both <see cref="ICanChangeMemoryProtection" /> and
    ///     <see cref="ICanReadWriteMemory" />.
    /// </typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SafeWrite<TMemory>(this TMemory item, nuint memoryAddress, Span<byte> data)
        where TMemory : ICanChangeMemoryProtection, ICanReadWriteMemory
    {
        using DisposableMemoryProtection<TMemory> permissions =
            item.ChangeProtectionDisposable(memoryAddress, data.Length, MemoryProtection.ReadWriteExecute);
        item.WriteRaw(memoryAddress, data);
    }

    /// <summary>
    ///     Reads data into the span, temporarily changing the memory permissions to read/write/execute.
    /// </summary>
    /// <param name="item">
    ///     Item inheriting from both <see cref="ICanChangeMemoryProtection" /> and
    ///     <see cref="ICanReadWriteMemory" />.
    /// </param>
    /// <param name="memoryAddress">The memory address to write to.</param>
    /// <param name="data">Data to write to the address.</param>
    /// <typeparam name="TMemory">
    ///     Item inheriting from both <see cref="ICanChangeMemoryProtection" /> and
    ///     <see cref="ICanReadWriteMemory" />.
    /// </typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SafeRead<TMemory>(this TMemory item, nuint memoryAddress, Span<byte> data)
        where TMemory : ICanChangeMemoryProtection, ICanReadWriteMemory
    {
        using DisposableMemoryProtection<TMemory> permissions =
            item.ChangeProtectionDisposable(memoryAddress, data.Length, MemoryProtection.ReadWriteExecute);
        item.ReadRaw(memoryAddress, data);
    }
}
