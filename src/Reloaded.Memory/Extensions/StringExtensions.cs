using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Internals;
using Reloaded.Memory.Internals.Algorithms;
using Reloaded.Memory.Utilities.License;

#if NET7_0_OR_GREATER
using Reloaded.Memory.Internals.Backports.System.Globalization;
#endif

namespace Reloaded.Memory.Extensions;

/// <summary>
///     Helpers for working with the <see cref="string" /> type.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Returns a reference to the first element within a given <see cref="string" />, with no bounds checks.
    /// </summary>
    /// <param name="text">The input <see cref="string" /> instance.</param>
    /// <returns>
    ///     A reference to the first element within <paramref name="text" />, or the location it would have used, if
    ///     <paramref name="text" /> is empty.
    /// </returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in
    ///     case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
    [MITLicense]
    public static ref char DangerousGetReference(this string text)
    {
#if NET8_0_OR_GREATER
        return ref Unsafe.AsRef(in text.GetPinnableReference());
#elif NET6_0_OR_GREATER
        return ref Unsafe.AsRef(text.GetPinnableReference());
#else
        return ref MemoryMarshal.GetReference(text.AsSpan());
#endif
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="string" />, with no bounds checks.
    /// </summary>
    /// <param name="text">The input <see cref="string" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="text" />.</param>
    /// <returns>A reference to the element within <paramref name="text" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
    [MITLicense]
    public static ref char DangerousGetReferenceAt(this string text, int i)
    {
#if NET8_0_OR_GREATER
        ref char r0 = ref Unsafe.AsRef(in text.GetPinnableReference());
#elif NET6_0_OR_GREATER
        ref char r0 = ref Unsafe.AsRef(text.GetPinnableReference());
#else
        ref var r0 = ref MemoryMarshal.GetReference(text.AsSpan());
#endif
        ref var ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    ///     Counts the number of occurrences of a given character into a target <see cref="string" /> instance.
    /// </summary>
    /// <param name="text">The input <see cref="string" /> instance to read.</param>
    /// <param name="c">The character to look for.</param>
    /// <returns>The number of occurrences of <paramref name="c" /> in <paramref name="text" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
    [MITLicense]
    public static int Count(this string text, char c)
    {
        ref var r0 = ref text.DangerousGetReference();
        var length = (nint)(uint)text.Length;

        return (int)SpanHelper.Count(ref r0, length, c);
    }

    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    ///
    ///     This method does not provide guarantees about producing the same hash across different machines or library versions,
    ///     or runtime; only for the current process. Instead, it prioritises speed over all.
    /// </remarks>
    public static nuint GetHashCodeFast(this string text) => text.AsSpan().GetHashCodeFast();

    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    ///
    ///     This method does not provide guarantees about producing the same hash across different machines or library versions,
    ///     or runtime; only for the current process. Instead, it prioritises speed over all.
    /// </remarks>
    [ExcludeFromCodeCoverage] // "Cannot be accurately measured without multiple architectures. Known good impl." This is still tested tho.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe nuint GetHashCodeFast(this ReadOnlySpan<char> text) => text.GetHashCodeUnstable();

    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    ///     Hashes the string in lower (invariant) case.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    ///
    ///     This method does not provide guarantees about producing the same hash across different machines or library versions,
    ///     or runtime; only for the current process. Instead, it prioritises speed over all.
    /// </remarks>
    public static nuint GetHashCodeLowerFast(this string text) => text.AsSpan().GetHashCodeLowerFast();

    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    ///     Hashes the string in lower (invariant) case.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    ///
    ///     This method does not provide guarantees about producing the same hash across different machines or library versions,
    ///     or runtime; only for the current process. Instead, it prioritises speed over all.
    /// </remarks>
    [ExcludeFromCodeCoverage] // "Cannot be accurately measured without multiple architectures. Known good impl." This is still tested tho.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe nuint GetHashCodeLowerFast(this ReadOnlySpan<char> text) => text.GetHashCodeUnstableLower();

#if NET7_0_OR_GREATER
    /// <summary>
    ///     Converts the given string to lower case (invariant casing), using the fastest possible implementation.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     This method is currently unoptimized for short non-ASCII inputs (due to runtime API limitations).
    ///     This will be worked around in the future.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToLowerInvariantFast(this string text) => TextInfo.ChangeCase<TextInfo.ToLowerConversion>(text);

    /// <summary>
    ///     Converts the given string to lower case (invariant casing), using the fastest possible implementation.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <param name="target"></param>
    /// <remarks>
    ///     This method is currently unoptimized for short non-ASCII inputs (due to runtime API limitations).
    ///     This will be worked around in the future.
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToLowerInvariantFast(this ReadOnlySpan<char> text, Span<char> target) => TextInfo.ChangeCase<TextInfo.ToLowerConversion>(text, target);

    /// <summary>
    ///     Converts the given string to upper case (invariant casing), using the fastest possible implementation.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     This method is currently unoptimized for short non-ASCII inputs (due to runtime API limitations).
    ///     This will be worked around in the future.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToUpperInvariantFast(this string text) => TextInfo.ChangeCase<TextInfo.ToUpperConversion>(text);

    /// <summary>
    ///     Converts the given string to upper case (invariant casing), using the fastest possible implementation.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <param name="target">The destination where the new string should be written.</param>
    /// <remarks>
    ///     This method is currently unoptimized for short non-ASCII inputs (due to runtime API limitations).
    ///     This will be worked around in the future.
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ToUpperInvariantFast(this ReadOnlySpan<char> text, Span<char> target) => TextInfo.ChangeCase<TextInfo.ToUpperConversion>(text, target);
#endif
}
