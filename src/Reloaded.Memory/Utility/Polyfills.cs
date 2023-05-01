namespace Reloaded.Memory.Utility;

/// <summary>
///     Provides backward compatibility support for older .NET versions.
/// </summary>
/// <remarks>
///     In cases where the feature is not directly supported, a best effort alternative is provided.
/// </remarks>
public static class Polyfills
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
}
