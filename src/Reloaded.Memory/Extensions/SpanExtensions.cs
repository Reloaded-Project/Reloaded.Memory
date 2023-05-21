using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Reloaded.Memory.Internals;
using Reloaded.Memory.Utilities.License;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace Reloaded.Memory.Extensions;

/// <summary>
///     Helpers for working with the <see cref="Span{T}" /> type.
/// </summary>
[PublicAPI]
public static class SpanExtensions
{
    /// <summary>
    ///     Casts a span to another type without bounds checks.
    /// </summary>
    /// <typeparam name="TFrom">Item to cast from.</typeparam>
    /// <typeparam name="TTo">Item to cast to.</typeparam>
    /// <param name="data">Data to cast.</param>
    /// <remarks>
    ///     Not accelerated on .NET Framework and Net Standard 2.0.
    /// </remarks>
    [MITLicense]
    [ExcludeFromCodeCoverage] // "Taken from runtime."
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<TTo> CastFast<TFrom, TTo>(this Span<TFrom> data) where TFrom : struct where TTo : struct
    {
#if NETSTANDARD2_0 || NET48
        return MemoryMarshal.Cast<TFrom, TTo>(data);
#else
        // Taken from the runtime.
        // Use unsigned integers - unsigned division by constant (especially by power of 2)
        // and checked casts are faster and smaller.
        var fromSize = (uint)Unsafe.SizeOf<TFrom>();
        var toSize = (uint)Unsafe.SizeOf<TTo>();
        var fromLength = (uint)data.Length;
        int toLength;
        if (fromSize == toSize)
        {
            // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
            // should be optimized to just `length` but the JIT doesn't do that today.
            toLength = (int)fromLength;
        }
        else if (fromSize == 1)
        {
            // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
            // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
            // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
            // the JIT can't eliminate long multiply by 1.
            toLength = (int)(fromLength / toSize);
        }
        else
        {
            // Ensure that casts are done in such a way that the JIT is able to "see"
            // the uint->ulong casts and the multiply together so that on 32 bit targets
            // 32x32to64 multiplication is used.
            var toLengthUInt64 = fromLength * (ulong)fromSize / toSize;
            toLength = (int)toLengthUInt64;
        }

        return MemoryMarshal.CreateSpan(
            ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(data)),
            toLength);
#endif
    }

    /// <summary>
    ///     Casts a span to another type without bounds checks.
    /// </summary>
    /// <typeparam name="TFrom">Item to cast from.</typeparam>
    /// <typeparam name="TTo">Item to cast to.</typeparam>
    /// <param name="data">Data to cast.</param>
    /// <remarks>
    ///     Not accelerated on .NET Framework and Net Standard 2.0.
    /// </remarks>
    [MITLicense]
    [ExcludeFromCodeCoverage] // "Taken from runtime."
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<TTo> CastFast<TFrom, TTo>(this ReadOnlySpan<TFrom> data)
        where TFrom : struct where TTo : struct
    {
#if NETSTANDARD2_0 || NET48
        return MemoryMarshal.Cast<TFrom, TTo>(data);
#else
        // Taken from the runtime.
        // Use unsigned integers - unsigned division by constant (especially by power of 2)
        // and checked casts are faster and smaller.
        var fromSize = (uint)Unsafe.SizeOf<TFrom>();
        var toSize = (uint)Unsafe.SizeOf<TTo>();
        var fromLength = (uint)data.Length;
        int toLength;
        if (fromSize == toSize)
        {
            // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
            // should be optimized to just `length` but the JIT doesn't do that today.
            toLength = (int)fromLength;
        }
        else if (fromSize == 1)
        {
            // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
            // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
            // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
            // the JIT can't eliminate long multiply by 1.
            toLength = (int)(fromLength / toSize);
        }
        else
        {
            // Ensure that casts are done in such a way that the JIT is able to "see"
            // the uint->ulong casts and the multiply together so that on 32 bit targets
            // 32x32to64 multiplication is used.
            var toLengthUInt64 = fromLength * (ulong)fromSize / toSize;
            toLength = (int)toLengthUInt64;
        }

        return MemoryMarshal.CreateSpan(
            ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(data)),
            toLength);
#endif
    }

    /// <summary>
    ///     Returns a reference to the first element within a given <see cref="Span{T}" />, with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="Span{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance.</param>
    /// <returns>A reference to the first element within <paramref name="span" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in
    ///     case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ref T DangerousGetReference<T>(this Span<T> span) => ref MemoryMarshal.GetReference(span);

    /// <summary>
    ///     Returns a reference to the first element within a given <see cref="ReadOnlySpan{T}" />, with no bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="ReadOnlySpan{T}" /> instance.</param>
    /// <returns>A reference to the first element within <paramref name="span" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in
    ///     case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ref T DangerousGetReference<T>(this ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference(span);

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="Span{T}" />, with no bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="Span{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ref T DangerousGetReferenceAt<T>(this Span<T> span, int i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="ReadOnlySpan{T}" />, with no
    ///     bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="ReadOnlySpan{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ref T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, int i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="Span{T}" />, with no bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="Span{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ref T DangerousGetReferenceAt<T>(this Span<T> span, nint i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, i);

        return ref ri;
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="ReadOnlySpan{T}" />, with no
    ///     bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="ReadOnlySpan{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ref T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, nint i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, i);

        return ref ri;
    }

    /// <summary>
    ///     Casts a <see cref="Span{T}" /> of one primitive type <typeparamref name="T" /> to <see cref="Span{T}" /> of bytes.
    /// </summary>
    /// <typeparam name="T">The type if items in the source <see cref="Span{T}" />.</typeparam>
    /// <param name="span">The source slice, of type <typeparamref name="T" />.</param>
    /// <returns>A <see cref="Span{T}" /> of bytes.</returns>
    /// <exception cref="OverflowException">
    ///     Thrown if the <see cref="Span{T}.Length" /> property of the new <see cref="Span{T}" /> would exceed
    ///     <see cref="int.MaxValue" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static Span<byte> AsBytes<T>(this Span<T> span)
        where T : unmanaged => MemoryMarshal.AsBytes(span);

    /// <summary>
    ///     Casts a <see cref="ReadOnlySpan{T}" /> of one primitive type <typeparamref name="T" /> to
    ///     <see cref="ReadOnlySpan{T}" /> of bytes.
    /// </summary>
    /// <typeparam name="T">The type if items in the source <see cref="ReadOnlySpan{T}" />.</typeparam>
    /// <param name="span">The source slice, of type <typeparamref name="T" />.</param>
    /// <returns>A <see cref="Span{T}" /> of bytes.</returns>
    /// <exception cref="OverflowException">
    ///     Thrown if the <see cref="Span{T}.Length" /> property of the new <see cref="ReadOnlySpan{T}" /> would exceed
    ///     <see cref="int.MaxValue" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ReadOnlySpan<byte> AsBytes<T>(this ReadOnlySpan<T> span)
        where T : unmanaged => MemoryMarshal.AsBytes(span);

    /// <summary>
    ///     Casts a <see cref="Span{T}" /> of one primitive type <typeparamref name="TFrom" /> to another primitive type
    ///     <typeparamref name="TTo" />.
    /// </summary>
    /// <typeparam name="TFrom">The type of items in the source <see cref="Span{T}" />.</typeparam>
    /// <typeparam name="TTo">The type of items in the destination <see cref="Span{T}" />.</typeparam>
    /// <param name="span">The source slice, of type <typeparamref name="TFrom" />.</param>
    /// <returns>A <see cref="Span{T}" /> of type <typeparamref name="TTo" /></returns>
    /// <remarks>
    ///     Supported only for platforms that support misaligned memory access or when the memory block is aligned by other
    ///     means.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static Span<TTo> Cast<TFrom, TTo>(this Span<TFrom> span)
        where TFrom : unmanaged
        where TTo : unmanaged => MemoryMarshal.Cast<TFrom, TTo>(span);

    /// <summary>
    ///     Casts a <see cref="ReadOnlySpan{T}" /> of one primitive type <typeparamref name="TFrom" /> to another primitive
    ///     type
    ///     <typeparamref name="TTo" />.
    /// </summary>
    /// <typeparam name="TFrom">The type of items in the source <see cref="ReadOnlySpan{T}" />.</typeparam>
    /// <typeparam name="TTo">The type of items in the destination <see cref="ReadOnlySpan{T}" />.</typeparam>
    /// <param name="span">The source slice, of type <typeparamref name="TFrom" />.</param>
    /// <returns>A <see cref="Span{T}" /> of type <typeparamref name="TTo" /></returns>
    /// <remarks>
    ///     Supported only for platforms that support misaligned memory access or when the memory block is aligned by other
    ///     means.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(this ReadOnlySpan<TFrom> span)
        where TFrom : unmanaged
        where TTo : unmanaged => MemoryMarshal.Cast<TFrom, TTo>(span);

    /// <summary>
    ///     Gets the index of an element of a given <see cref="Span{T}" /> from its reference.
    /// </summary>
    /// <typeparam name="T">The type if items in the input <see cref="Span{T}" />.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> to calculate the index for.</param>
    /// <param name="value">The reference to the target item to get the index for.</param>
    /// <returns>The index of <paramref name="value" /> within <paramref name="span" />, or <c>-1</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static unsafe int IndexOf<T>(this Span<T> span, ref T value)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        IntPtr byteOffset = Unsafe.ByteOffset(ref r0, ref value);

#pragma warning disable CS8500
        nint elementOffset = byteOffset / (nint)(uint)sizeof(T);
#pragma warning restore CS8500

        if ((nuint)elementOffset >= (uint)span.Length)
            return -1;

        return (int)elementOffset;
    }

    /// <summary>
    ///     Gets the index of an element of a given <see cref="ReadOnlySpan{T}" /> from its reference.
    /// </summary>
    /// <typeparam name="T">The type if items in the input <see cref="ReadOnlySpan{T}" />.</typeparam>
    /// <param name="span">The input <see cref="ReadOnlySpan{T}" /> to calculate the index for.</param>
    /// <param name="value">The reference to the target item to get the index for.</param>
    /// <returns>The index of <paramref name="value" /> within <paramref name="span" />, or <c>-1</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static unsafe int IndexOf<T>(this ReadOnlySpan<T> span, ref T value)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        IntPtr byteOffset = Unsafe.ByteOffset(ref r0, ref value);

#pragma warning disable CS8500
        nint elementOffset = byteOffset / (nint)(uint)sizeof(T);
#pragma warning restore CS8500

        if ((nuint)elementOffset >= (uint)span.Length)
            return -1;

        return (int)elementOffset;
    }

    /// <summary>
    ///     Counts the number of occurrences of a given value into a target <see cref="Span{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type of items in the input <see cref="Span{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance to read.</param>
    /// <param name="value">The <typeparamref name="T" /> value to look for.</param>
    /// <returns>The number of occurrences of <paramref name="value" /> in <paramref name="span" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static int Count<T>(this Span<T> span, T value)
        where T : IEquatable<T>
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        var length = (nint)(uint)span.Length;
        return (int)SpanHelper.Count(ref r0, length, value);
    }

    /// <summary>
    ///     Counts the number of occurrences of a given value into a target <see cref="ReadOnlySpan{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance to read.</param>
    /// <param name="value">The <typeparamref name="T" /> value to look for.</param>
    /// <returns>The number of occurrences of <paramref name="value" /> in <paramref name="span" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit
    [MITLicense]
    public static int Count<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        var length = (nint)(uint)span.Length;
        return (int)SpanHelper.Count(ref r0, length, value);
    }

    /// <summary>
    ///     Slices a span without any bounds checks, using a start index and length.
    /// </summary>
    /// <param name="data">The input span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <param name="length">The number of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> SliceFast<T>(this Span<T> data, int start, int length)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start), length);
#else
        return data.Slice(start, length);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a start index and length.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <param name="length">The number of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, int start, int length)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start), length);
#else
        return data.Slice(start, length);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a range.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="range">The range of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    /// <remarks>
    ///     This is a fallback. Normally the compiler will lower this to the overload with index and length.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, Range range)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref MemoryMarshal.GetReference(data), range.Start.GetOffset(data.Length)),
            range.End.Value - range.Start.Value);
#else
        return data.Slice(range.Start.GetOffset(data.Length), range.End.Value - range.Start.Value);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a range.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="range">The range of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    /// <remarks>
    ///     This is a fallback. Normally the compiler will lower this to the overload with index and length.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> SliceFast<T>(this Span<T> data, Range range)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref MemoryMarshal.GetReference(data), range.Start.GetOffset(data.Length)),
            range.End.Value - range.Start.Value);
#else
        return data.Slice(range.Start.GetOffset(data.Length), range.End.Value - range.Start.Value);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a start index.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> SliceFast<T>(this Span<T> data, int start)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start),
            data.Length - start);
#else
        return data.Slice(start);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a start index.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, int start)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start),
            data.Length - start);
#else
        return data.Slice(start);
#endif
    }

    /// <summary>
    ///     Replaces the occurrences of one character with another in a span.
    /// </summary>
    /// <param name="data">The data to replace the value in.</param>
    /// <param name="oldValue">The original value to be replaced.</param>
    /// <param name="newValue">The new replaced value.</param>
    /// <param name="buffer">
    ///     The buffer to place the result in.
    ///     This can be the original <paramref name="data" /> buffer if required.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> Replace(this Span<char> data, char oldValue, char newValue, Span<char> buffer) =>
        // char is not supported by Vector; but ushort is.
        Replace(data.CastFast<char, ushort>(), oldValue, newValue, buffer.CastFast<char, ushort>())
            .CastFast<ushort, char>();

    /// <summary>
    ///     Replaces the occurrences of one value with another in a span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">The data to replace the value in.</param>
    /// <param name="oldValue">The original value to be replaced.</param>
    /// <param name="newValue">The new replaced value.</param>
    /// <param name="buffer">
    ///     The buffer to place the result in.
    ///     This can be the original <paramref name="data" /> buffer if required.
    /// </param>
    /// <typeparamref name="T">MUST BE POWER OF TWO IN SIZE. Type of value to replace.</typeparamref>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<T> Replace<T>(this Span<T> data, T oldValue, T newValue, Span<T> buffer)
        where T : unmanaged, IEquatable<T>
    {
        // In the case they are the same, do nothing.
        if (oldValue.Equals(newValue))
            return data;

        // No-op on empty span.
        if (data.IsEmpty)
            return data;

        // This is evaluated at JIT time :)
        if ((sizeof(T) & (sizeof(T) - 1)) != 0)
            throw new Exception("Generic type T must be a power of 2.");

        // Slice our output buffer.
        buffer = buffer.SliceFast(0, data.Length);
        var remainingLength = (nuint)data.Length;

        // Copy the remaining characters, doing the replacement as we go.
        // Note: We can index 0 directly since we know length is >0 given length check from earlier.
        ref T pSrc = ref data[0];
        ref T pDst = ref buffer[0];
        nuint x = 0;

        if (Vector.IsHardwareAccelerated && data.Length >= Vector<T>.Count)
        {
            Vector<T> oldValues = new(oldValue);
            Vector<T> newValues = new(newValue);

            Vector<T> original;
            Vector<T> equals;
            Vector<T> results;

            if (remainingLength > (nuint)Vector<T>.Count)
            {
                nuint lengthToExamine = remainingLength - (nuint)Vector<T>.Count;

                do
                {
                    original = VectorExtensions.LoadUnsafe(ref pSrc, x);
                    equals = Vector.Equals(original, oldValues); // Generate Mask
                    results = Vector.ConditionalSelect(equals, newValues, original); // Swap in Values
                    results.StoreUnsafe(ref pDst, x);

                    x += (nuint)Vector<T>.Count;
                } while (x < lengthToExamine);
            }

            // There are between 0 to Vector<T>.Count elements remaining now.

            // Since our operation can be applied multiple times without changing the result
            // [applying the replacement twice is non destructive]. We can avoid non-vectorised code
            // here and simply do the vectorised logic in an unaligned fashion, doing just the chunk
            // at the end of the original buffer.
            x = (uint)(data.Length - Vector<T>.Count);
            original = VectorExtensions.LoadUnsafe(ref data[0], x);
            equals = Vector.Equals(original, oldValues);
            results = Vector.ConditionalSelect(equals, newValues, original);
            results.StoreUnsafe(ref buffer[0], x);
        }
        else
        {
            // Non-vector fallback, slow.
            for (; x < remainingLength; ++x)
            {
                T currentChar = Unsafe.Add(ref pSrc, (nint)x);
                Unsafe.Add(ref pDst, (nint)x) = currentChar.Equals(oldValue) ? newValue : currentChar;
            }
        }

        return buffer;
    }

    /// <summary>
    ///     Finds all offsets of a given value within the specified data.
    /// </summary>
    /// <param name="data">The data to search within.</param>
    /// <param name="value">Value to listen to.</param>
    /// <returns>A list of all offsets of a given value within the span.</returns>
    public static List<int> FindAllOffsetsOfByte(this ReadOnlySpan<byte> data, byte value)
        => FindAllOffsetsOfByte(data, value, 32);

    /// <summary>
    ///     Finds all offsets of a given value within the specified data.
    /// </summary>
    /// <param name="data">The data to search within.</param>
    /// <param name="value">Value to listen to.</param>
    /// <param name="offsetCountHint">Hint for likely amount of offsets.</param>
    /// <returns>A list of all offsets of a given value within the span.</returns>
    public static unsafe List<int> FindAllOffsetsOfByte(this ReadOnlySpan<byte> data, byte value, int offsetCountHint)
    {
        // Note: A generic implementation wouldn't look too different here; just would need another fallback in case
        // sizeof(T) is bigger than nint.

        // TODO: Unrolled CPU version for non-AMD64 platforms.
        // Note: I wrote this in SSE/AVX directly because System.Numerics.Vectors does not have equivalent of MoveMask
        //       which means getting offset of matched value is slow.
        var offsets = new List<int>(offsetCountHint);
        fixed (byte* dataPtr = data)
        {
#if NETCOREAPP3_1_OR_GREATER
            if (Avx2.IsSupported)
            {
                FindAllOffsetsOfByteAvx2(dataPtr, data.Length, value, offsets);
                return offsets;
            }

            if (Sse2.IsSupported) // all AMD64 CPUs
            {
                FindAllOffsetsOfByteSse2(dataPtr, data.Length, value, offsets);
                return offsets;
            }
#endif

            // Otherwise probably not a x64 CPU.
            FindAllOffsetsOfByteFallback(dataPtr, data.Length, value, 0, offsets);
            return offsets;
        }
    }

    internal static unsafe void FindAllOffsetsOfByteFallback(byte* data, int length, byte value, int addToResults,
        List<int> results)
    {
        const int unrollFactor = 8;
        var dataPtr = data;
        var dataMaxPtr = dataPtr + length;

        // Unroll the loop
        while (dataPtr + unrollFactor <= dataMaxPtr)
        {
            if (dataPtr[0] == value) results.Add((int)(dataPtr - data) + addToResults);
            if (dataPtr[1] == value) results.Add((int)(dataPtr - data + 1) + addToResults);
            if (dataPtr[2] == value) results.Add((int)(dataPtr - data + 2) + addToResults);
            if (dataPtr[3] == value) results.Add((int)(dataPtr - data + 3) + addToResults);
            if (dataPtr[4] == value) results.Add((int)(dataPtr - data + 4) + addToResults);
            if (dataPtr[5] == value) results.Add((int)(dataPtr - data + 5) + addToResults);
            if (dataPtr[6] == value) results.Add((int)(dataPtr - data + 6) + addToResults);
            if (dataPtr[7] == value) results.Add((int)(dataPtr - data + 7) + addToResults);

            dataPtr += unrollFactor;
        }

        // Process remaining elements
        while (dataPtr < dataMaxPtr)
        {
            if (*dataPtr == value) results.Add((int)(dataPtr - data) + addToResults);
            dataPtr++;
        }
    }

#if NETCOREAPP3_1_OR_GREATER
    internal static unsafe void FindAllOffsetsOfByteAvx2(byte* data, int length, byte value, List<int> results)
    {
        const int avxRegisterLength = 32;

        // Byte to search for.
        Vector256<byte> byteVec = Vector256.Create(value);
        var dataPtr = data;
        var dataMaxPtr = dataPtr + (length - avxRegisterLength);
        var simdJump = avxRegisterLength - 1;

        while (dataPtr < dataMaxPtr)
        {
            Vector256<byte> rhs = Avx.LoadVector256(dataPtr);
            Vector256<byte> equal = Avx2.CompareEqual(byteVec, rhs);
            var findFirstByte = Avx2.MoveMask(equal);

            // All 0s, so none of them had desired value.
            if (findFirstByte == 0)
            {
                dataPtr += simdJump;
                continue;
            }

            // Shift up until first byte found.
            dataPtr += BitOperations.TrailingZeroCount((uint)findFirstByte);
            results.Add((int)(dataPtr - data));
            dataPtr++; // go to next element
        }

        // Check last few bytes using byte by byte comparison.
        var position = (int)(dataPtr - data);
        FindAllOffsetsOfByteFallback(data + position, length - position, value, position, results);
    }

    internal static unsafe void FindAllOffsetsOfByteSse2(byte* data, int length, byte value, List<int> results)
    {
        const int sseRegisterLength = 16;

        // Byte to search for.
        Vector128<byte> byteVec = Vector128.Create(value);
        var dataPtr = data;
        var dataMaxPtr = dataPtr + (length - sseRegisterLength);
        var simdJump = sseRegisterLength - 1;

        while (dataPtr < dataMaxPtr)
        {
            Vector128<byte> rhs = Sse2.LoadVector128(dataPtr);
            Vector128<byte> equal = Sse2.CompareEqual(byteVec, rhs);
            var findFirstByte = Sse2.MoveMask(equal);

            // All 0s, so none of them had desired value.
            if (findFirstByte == 0)
            {
                dataPtr += simdJump;
                continue;
            }

            // Shift up until first byte found.
            dataPtr += BitOperations.TrailingZeroCount((uint)findFirstByte);
            results.Add((int)(dataPtr - data));
            dataPtr++; // go to next element
        }

        // Check last few bytes using byte by byte comparison.
        var position = (int)(dataPtr - data);
        FindAllOffsetsOfByteFallback(data + position, length - position, value, position, results);
    }
#endif
}
