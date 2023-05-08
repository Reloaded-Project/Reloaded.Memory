// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Internals;
#if NETSTANDARD
using System.Runtime.InteropServices;
#endif

namespace Reloaded.Memory.Extensions;

/// <summary>
/// Helpers for working with the <see cref="string"/> type.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
public static class StringExtensions
{
    /// <summary>
    /// Returns a reference to the first element within a given <see cref="string"/>, with no bounds checks.
    /// </summary>
    /// <param name="text">The input <see cref="string"/> instance.</param>
    /// <returns>A reference to the first element within <paramref name="text"/>, or the location it would have used, if <paramref name="text"/> is empty.</returns>
    /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char DangerousGetReference(this string text)
    {
#if NET6_0_OR_GREATER
        return ref Unsafe.AsRef(text.GetPinnableReference());
#else
        return ref MemoryMarshal.GetReference(text.AsSpan());
#endif
    }

    /// <summary>
    /// Returns a reference to an element at a specified index within a given <see cref="string"/>, with no bounds checks.
    /// </summary>
    /// <param name="text">The input <see cref="string"/> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="text"/>.</param>
    /// <returns>A reference to the element within <paramref name="text"/> at the index specified by <paramref name="i"/>.</returns>
    /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char DangerousGetReferenceAt(this string text, int i)
    {
#if NET6_0_OR_GREATER
        ref char r0 = ref Unsafe.AsRef(text.GetPinnableReference());
#else
        ref char r0 = ref MemoryMarshal.GetReference(text.AsSpan());
#endif
        ref char ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    /// Counts the number of occurrences of a given character into a target <see cref="string"/> instance.
    /// </summary>
    /// <param name="text">The input <see cref="string"/> instance to read.</param>
    /// <param name="c">The character to look for.</param>
    /// <returns>The number of occurrences of <paramref name="c"/> in <paramref name="text"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(this string text, char c)
    {
        ref char r0 = ref text.DangerousGetReference();
        nint length = (nint)(uint)text.Length;

        return (int)SpanHelper.Count(ref r0, length, c);
    }
}
