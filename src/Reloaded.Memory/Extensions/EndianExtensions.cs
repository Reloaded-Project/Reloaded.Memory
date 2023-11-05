using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Extensions;

/// <summary>
///     Extension methods resolving around Endian conversion
/// </summary>
[ExcludeFromCodeCoverage]
[PublicAPI]
public static class EndianExtensions
{
    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte AsLittleEndian(this byte value) => value;

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte AsLittleEndian(this sbyte value) => value;

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short AsLittleEndian(this short value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort AsLittleEndian(this ushort value) =>
        BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AsLittleEndian(this int value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint AsLittleEndian(this uint value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long AsLittleEndian(this long value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong AsLittleEndian(this ulong value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AsLittleEndian(this float value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AsLittleEndian(this double value) => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte AsBigEndian(this byte value) => value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte AsBigEndian(this sbyte value) => value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short AsBigEndian(this short value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort AsBigEndian(this ushort value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AsBigEndian(this int value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint AsBigEndian(this uint value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long AsBigEndian(this long value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong AsBigEndian(this ulong value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AsBigEndian(this float value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AsBigEndian(this double value) => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to big endian.
    ///     On big endian this is a no-op. On little endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    /// <typeparam name="T">Type of value to convert.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToBigEndian<T>(T value) where T : unmanaged => BitConverter.IsLittleEndian ? Endian.Reverse(value) : value;

    /// <summary>
    ///     Converts <see cref="value"/> to little endian.
    ///     On little endian this is a no-op. On big endian the bytes are swapped.
    /// </summary>
    /// <param name="value">The value whose endian to convert.</param>
    /// <typeparam name="T">Type of value to convert.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToLittleEndian<T>(T value) where T : unmanaged => BitConverter.IsLittleEndian ? value : Endian.Reverse(value);
}
