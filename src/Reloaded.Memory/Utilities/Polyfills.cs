#if NET7_0_OR_GREATER
#else
using Reloaded.Memory.Exceptions;
#endif
using Reloaded.Memory.Extensions;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     Provides backward compatibility support for older .NET versions.
/// </summary>
/// <remarks>
///     In cases where the feature is not directly supported, a best effort alternative is provided.
/// </remarks>
internal static class Polyfills
{
    // The OS identifier platform code below is JIT friendly; compiled out at runtime for .NET 5 and above.

    /// <summary>
    ///     Returns true if the current operating system is Windows.
    /// </summary>
    public static bool IsWindows()
    {
#if NET5_0_OR_GREATER
        return OperatingSystem.IsWindows();
#else
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
    }

    /// <summary>
    ///     Returns true if the current operating system is Linux.
    /// </summary>
    public static bool IsLinux()
    {
#if NET5_0_OR_GREATER
        return OperatingSystem.IsLinux();
#else
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
    }

    /// <summary>
    ///     Returns true if the current operating system is MacOS.
    /// </summary>
    public static bool IsMacOS()
    {
#if NET5_0_OR_GREATER
        return OperatingSystem.IsMacOS();
#else
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
    }

    /// <summary>
    ///     Allocates an array without zero filling it.
    /// </summary>
    /// <typeparam name="T">Type of item to return array of.</typeparam>
    /// <param name="size">Number of items to return.</param>
    /// <param name="pinned">Whether the data should be pinned or not.</param>
    /// <returns>Array of requested items.</returns>
    public static T[] AllocateUninitializedArray<T>(int size, bool pinned = false)
    {
#if NET5_0_OR_GREATER
        return GC.AllocateUninitializedArray<T>(size, pinned);
#else
        return new T[size];
#endif
    }

    /// <summary>
    ///     Appends a span of bytes onto the <see cref="Stream" /> and advances the position.
    ///     This is a polyfill for older framework versions.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <param name="stream">The stream to write the result to.</param>
    /// <param name="buffer">The bytes to write to the output.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<TStream>(this TStream stream, Span<byte> buffer) where TStream : Stream
    {
#if NETCOREAPP3_1_OR_GREATER || NETSTANDARD2_1
        stream.Write(buffer);
#else
        using var rental = new ArrayRental(buffer.Length);
        Span<byte> span = rental.Array.AsSpan(0, buffer.Length);
        buffer.CopyTo(span);
        stream.Write(rental.Array, 0, buffer.Length);
#endif
    }

    /// <summary>
    ///     Appends a span of bytes onto the <see cref="Stream" /> and advances the position.
    ///     This is a polyfill for ReadAtLeast, for older runtimes.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="buffer">The array to receive the result.</param>
    /// <param name="offset">Offset in the array.</param>
    /// <param name="length">Length in the array.</param>
    /// <param name="throwOnEndOfStream">Throws on end of stream.</param>
    /// <exception cref="EndOfStreamException">End of stream was reached.</exception>
    /// <returns>Number of bytes read.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadAtLeast<TStream>(this TStream stream, byte[] buffer, int offset, int length,
        bool throwOnEndOfStream = true) where TStream : Stream
    {
#if NET7_0_OR_GREATER
        return stream.ReadAtLeast(buffer.AsSpanFast(offset, length), length, throwOnEndOfStream);
#else
        using var rental = new ArrayRental(length);
        var totalRead = 0;
        while (totalRead < length)
        {
            var read = stream.Read(rental.Array, totalRead, length - totalRead);
            if (read == 0)
            {
                if (throwOnEndOfStream)
                    ThrowHelpers.ThrowEndOfFileException();

                rental.Array.AsSpan(0, totalRead).CopyTo(buffer.AsSpanFast(offset, totalRead));
                return totalRead;
            }

            totalRead += read;
        }

        rental.Array.AsSpan(0, totalRead).CopyTo(buffer.AsSpanFast(offset, totalRead));
        return totalRead;
#endif
    }

    /// <summary>
    ///     Appends a span of bytes onto the <see cref="Stream" /> and advances the position.
    ///     This is a polyfill for ReadAtLeast, for older runtimes.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="buffer">The span to receive the result from.</param>
    /// <param name="throwOnEndOfStream">Throws when end of stream encountered.</param>
    /// <returns>Number of bytes read.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadAtLeast<TStream>(this TStream stream, Span<byte> buffer, bool throwOnEndOfStream = false)
        where TStream : Stream
    {
#if NET7_0_OR_GREATER
        return stream.ReadAtLeast(buffer, buffer.Length, throwOnEndOfStream);
#else
        using var rental = new ArrayRental(buffer.Length);
        var totalRead = 0;
        while (totalRead < buffer.Length)
        {
            var read = stream.Read(rental.Array, totalRead, buffer.Length - totalRead);
            if (read == 0)
            {
                if (throwOnEndOfStream)
                    ThrowHelpers.ThrowEndOfFileException();

                rental.Array.AsSpan(0, totalRead).CopyTo(buffer);
                return totalRead;
            }

            totalRead += read;
        }

        rental.Array.AsSpan(0, buffer.Length).CopyTo(buffer);
        return totalRead;
#endif
    }
}
