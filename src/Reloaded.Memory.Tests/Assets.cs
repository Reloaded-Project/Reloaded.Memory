using System;
using System.IO;
using System.Runtime.InteropServices;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Tests;

public class Assets
{
    /// <summary>
    ///     Gets the path of the 'Hello World' executable.
    /// </summary>
    public static string GetHelloWorldExePath()
    {
        // TODO: Tests on ARM64 (GitHub Runner does not support ARM64)
        Architecture architecture = RuntimeInformation.ProcessArchitecture;
        if (Polyfills.IsWindows())
        {
            return architecture switch
            {
                Architecture.X86 => GetItemWithRelativePath("hello-world-win-x86.exe"),
                Architecture.X64 => GetItemWithRelativePath("hello-world-win-x64.exe"),
                Architecture.Arm64 => GetItemWithRelativePath("hello-world-win-arm64.exe"),
                _ => throw new PlatformNotSupportedException($"Windows with platform {architecture} not supported.")
            };
        }

        if (Polyfills.IsLinux())
        {
            return architecture switch
            {
                Architecture.X86 => GetItemWithRelativePath("hello-world-linux-x86.elf"),
                Architecture.X64 => GetItemWithRelativePath("hello-world-linux-x64.elf"),
                Architecture.Arm64 => GetItemWithRelativePath("hello-world-win-arm64.exe"),
                _ => throw new PlatformNotSupportedException($"Linux with platform {architecture} not supported.")
            };
        }

        ThrowHelpers.ThrowPlatformNotSupportedException();
        return "";
    }

    /// <summary>
    ///     Gets an item with the specified relative path to the 'Assets' folder.
    /// </summary>
    public static string GetItemWithRelativePath(string path)
        => Path.Combine(AppContext.BaseDirectory, "Assets", path);
}
