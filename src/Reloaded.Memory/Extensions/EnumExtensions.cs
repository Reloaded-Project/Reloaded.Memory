namespace Reloaded.Memory.Extensions;

/// <summary>
///     Extension methods for dealing with enums.
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    /// <summary>
    ///     Determines if the given enum has a specified flag.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="flag">The flag to check.</param>
    /// <typeparam name="T">The type to check the flag of.</typeparam>
    /// <exception cref="NotSupportedException">This type of enum is not supported.</exception>
    /// <returns>True if the enum is contained in the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool HasFlagFast<T>(this T value, T flag) where T : unmanaged, Enum
    {
        if (sizeof(T) == sizeof(byte))
            return (Unsafe.As<T, byte>(ref value) & Unsafe.As<T, byte>(ref flag)) == Unsafe.As<T, byte>(ref flag);

        if (sizeof(T) == sizeof(short))
            return (Unsafe.As<T, short>(ref value) & Unsafe.As<T, short>(ref flag)) == Unsafe.As<T, short>(ref flag);

        if (sizeof(T) == sizeof(int))
            return (Unsafe.As<T, int>(ref value) & Unsafe.As<T, int>(ref flag)) == Unsafe.As<T, int>(ref flag);

        if (sizeof(T) == sizeof(long))
            return (Unsafe.As<T, long>(ref value) & Unsafe.As<T, long>(ref flag)) == Unsafe.As<T, long>(ref flag);

        return value.HasFlag(flag);
    }
}
