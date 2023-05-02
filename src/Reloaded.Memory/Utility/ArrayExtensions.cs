using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Utility;

/// <summary>
///     Extensions dealing with arrays.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    ///     Converts a byte array to a Span without doing a null check.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <returns>Created span, while omitting null check.</returns>
    public static Span<T> AsSpanFast<T>(this T[] data)
    {
#if NET5_0_OR_GREATER
        ref T reference = ref MemoryMarshal.GetArrayDataReference(data);
        return MemoryMarshal.CreateSpan(ref reference, data.Length);
#else
        return data.AsSpan();
#endif
    }
}
