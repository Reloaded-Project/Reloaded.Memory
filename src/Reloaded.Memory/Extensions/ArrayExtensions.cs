// Parts of this file are MIT licensed to the .NET Foundation and are taken from CommunityToolkit.HighPerformance.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Internals;
using RuntimeHelpers = Reloaded.Memory.Internals.RuntimeHelpers;
#if !NET5_0_OR_GREATER
using Reloaded.Memory.Utilities;
#endif

namespace Reloaded.Memory.Extensions;

/// <summary>
///     Extensions dealing with arrays.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Returns a reference to the first element within a given <typeparamref name="T" /> array, with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T" /> array instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T" /> array instance.</param>
    /// <returns>
    ///     A reference to the first element within <paramref name="array" />, or the location it would have used, if
    ///     <paramref name="array" /> is empty.
    /// </returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in
    ///     case the returned value is dereferenced.
    /// </remarks>
    [ExcludeFromCodeCoverage] // forked from CommunityToolkit
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReference<T>(this T[] array)
    {
#if NET5_0_OR_GREATER
        return ref MemoryMarshal.GetArrayDataReference(array);
#else
        IntPtr offset = RuntimeHelpers.GetArrayDataByteOffset<T>();
        return ref ObjectMarshal.DangerousGetObjectDataReferenceAt<T>(array, offset);
#endif
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <typeparamref name="T" /> array, with no
    ///     bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T" /> array instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T" /> array instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="array" />.</param>
    /// <returns>A reference to the element within <paramref name="array" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [ExcludeFromCodeCoverage] // forked from CommunityToolkit
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this T[] array, int i)
    {
#if NET5_0_OR_GREATER
        ref T r0 = ref MemoryMarshal.GetArrayDataReference(array);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
#else
        IntPtr offset = RuntimeHelpers.GetArrayDataByteOffset<T>();
        ref T r0 = ref ObjectMarshal.DangerousGetObjectDataReferenceAt<T>(array, offset);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
#endif
    }

    /// <summary>
    ///     Counts the number of occurrences of a given value into a target <typeparamref name="T" /> array instance.
    /// </summary>
    /// <typeparam name="T">The type of items in the input <typeparamref name="T" /> array instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T" /> array instance.</param>
    /// <param name="value">The <typeparamref name="T" /> value to look for.</param>
    /// <returns>The number of occurrences of <paramref name="value" /> in <paramref name="array" />.</returns>
    [ExcludeFromCodeCoverage] // forked from CommunityToolkit
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<T>(this T[] array, T value)
        where T : IEquatable<T>
    {
        ref T r0 = ref array.DangerousGetReference();
        nint length = RuntimeHelpers.GetArrayNativeLength(array);
        nint count = SpanHelper.Count(ref r0, length, value);

        if ((nuint)count > int.MaxValue)
            ThrowHelpers.ThrowOverflowException();

        return (int)count;
    }

    /// <summary>
    ///     Converts a byte array to a Span without doing a null check.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <returns>Created span, while omitting null check.</returns>
    /// <remarks>
    ///     Not accelerated on runtimes older than .NET 5.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpanFast<T>(this T[] data)
    {
#if NET5_0_OR_GREATER
        ref T reference = ref MemoryMarshal.GetArrayDataReference(data);
        return MemoryMarshal.CreateSpan(ref reference, data.Length);
#else
        return data.AsSpan();
#endif
    }

    /// <summary>
    ///     Converts a portion of a byte array to a Span without doing a null check, using an offset and a length.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <param name="offset">The zero-based starting index of the portion to convert.</param>
    /// <param name="length">The number of elements to include in the new Span.</param>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <returns>Created span, while omitting null check.</returns>
    /// <remarks>
    ///     Not accelerated on runtimes older than .NET 5.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpanFast<T>(this T[] data, int offset, int length)
    {
#if NET5_0_OR_GREATER
        ref T reference = ref MemoryMarshal.GetArrayDataReference(data);
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref reference, offset), length);
#else
        return data.AsSpan(offset, length);
#endif
    }

    /// <summary>
    ///     Converts a portion of a byte array to a Span without doing a null check, using an offset.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <param name="offset">The zero-based starting index of the portion to convert.</param>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <returns>Created span, while omitting null check.</returns>
    /// <remarks>
    ///     Not accelerated on runtimes older than .NET 5.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpanFast<T>(this T[] data, int offset)
    {
#if NET5_0_OR_GREATER
        ref T reference = ref MemoryMarshal.GetArrayDataReference(data);
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref reference, offset), data.Length - offset);
#else
        return data.AsSpan(offset);
#endif
    }

    /// <summary>
    ///     Converts a range within a byte array to a Span without doing a null check.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <param name="range">The range of elements to include in the new Span.</param>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <returns>Created span, while omitting null check.</returns>
    /// <remarks>
    ///     Not accelerated on runtimes older than .NET 5.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpanFast<T>(this T[] data, Range range)
    {
#if NET5_0_OR_GREATER
        var start = range.Start.GetOffset(data.Length);
        var length = range.End.GetOffset(data.Length) - start;
        ref T reference = ref MemoryMarshal.GetArrayDataReference(data);
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref reference, start), length);
#else
        return data.AsSpan(range.Start.Value, range.End.Value - range.Start.Value);
#endif
    }

    /// <summary>
    ///     Converts a portion of a byte array to a Span without doing a null check, using an index.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <param name="index">The zero-based starting index of the portion to convert.</param>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <returns>Created span, while omitting null check.</returns>
    /// <remarks>
    ///     Not accelerated on runtimes older than .NET 5.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpanFast<T>(this T[] data, Index index)
    {
#if NET5_0_OR_GREATER
        var offset = index.GetOffset(data.Length);
        ref T reference = ref MemoryMarshal.GetArrayDataReference(data);
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref reference, offset), data.Length - offset);
#else
        return data.AsSpan(index.Value);
#endif
    }

    /// <summary>
    ///     Checks whether or not a given <typeparamref name="T" /> array is covariant.
    /// </summary>
    /// <typeparam name="T">The type of items in the input <typeparamref name="T" /> array instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T" /> array instance.</param>
    /// <returns>Whether or not <paramref name="array" /> is covariant.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCovariant<T>(this T[] array) => default(T) is null && array.GetType() != typeof(T[]);
}
