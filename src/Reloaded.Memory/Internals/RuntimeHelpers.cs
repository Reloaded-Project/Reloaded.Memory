using System.Diagnostics.CodeAnalysis;
#if !NET5_0_OR_GREATER
using Reloaded.Memory.Utilities;
#endif

namespace Reloaded.Memory.Internals;

/// <summary>
///     A helper class that with utility methods for dealing with references, and other low-level details.
///     It also contains some APIs that act as polyfills for .NET Standard 2.0 and below.
/// </summary>
[ExcludeFromCodeCoverage] // From CommunityToolkit
internal static class RuntimeHelpers
{
    /// <summary>
    ///     Gets the length of a given array as a native integer.
    /// </summary>
    /// <typeparam name="T">The type of values in the array.</typeparam>
    /// <param name="array">The input <see cref="Array" /> instance.</param>
    /// <returns>The total length of <paramref name="array" /> as a native integer.</returns>
    /// <remarks>
    ///     This method is needed because this expression is not inlined correctly if the target array
    ///     is only visible as a non-generic <see cref="Array" /> instance, because the C# compiler will
    ///     not be able to emit the <see langword="ldlen" /> opcode instead of calling the right method.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint GetArrayNativeLength<T>(T[] array) => (nint)array.LongLength;

#if !NET5_0_OR_GREATER
    /// <summary>
    ///     Gets the byte offset to the first <typeparamref name="T" /> element in a SZ array.
    /// </summary>
    /// <typeparam name="T">The type of values in the array.</typeparam>
    /// <returns>The byte offset to the first <typeparamref name="T" /> element in a SZ array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr GetArrayDataByteOffset<T>() => TypeInfo<T>.ArrayDataByteOffset;

    /// <summary>
    ///     A private generic class to preload type info for arbitrary runtime types.
    /// </summary>
    /// <typeparam name="T">The type to load info for.</typeparam>
    private static class TypeInfo<T>
    {
        /// <summary>
        ///     The byte offset to the first <typeparamref name="T" /> element in a SZ array.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        public static readonly nint ArrayDataByteOffset = MeasureArrayDataByteOffset();

        /// <summary>
        ///     Computes the value for <see cref="ArrayDataByteOffset" />.
        /// </summary>
        /// <returns>The value of <see cref="ArrayDataByteOffset" /> for the current runtime.</returns>
        private static nint MeasureArrayDataByteOffset()
        {
            var array = new T[1];
            return ObjectMarshal.DangerousGetObjectDataByteOffset(array, ref array[0]);
        }
    }
#endif
}
