using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Reloaded.Memory.Memory;
using Reloaded.Memory.Memory.Interfaces;
using Reloaded.Memory.Utility;

namespace Reloaded.Memory.Tests.Utilities;

/// <summary>
///     Helper class for getting memory sources to test with.
/// </summary>
public class MemorySources
{
    public static ITemporaryMemorySource GetMemorySource(MemorySourceKind source) => source switch
    {
        MemorySourceKind.THIS_PROCESS => new MemoryMemorySource(),
        MemorySourceKind.EXTERNAL_PROCESS => new ExternalMemoryMemorySource(),
        _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
    };

    public enum MemorySourceKind
    {
        THIS_PROCESS,
        EXTERNAL_PROCESS
    }
}

public class MemorySourceKindNoExternalOnNonWindows : IEnumerable<object[]>
{
    private static IEnumerable<object[]> GetValues()
    {
        Array values = Enum.GetValues(typeof(MemorySources.MemorySourceKind));
        foreach (object? value in values)
        {
            if ((MemorySources.MemorySourceKind)value! == MemorySources.MemorySourceKind.EXTERNAL_PROCESS &&
                !Polyfills.IsWindows())
                continue;

            yield return new[] { value };
        }
    }

    public IEnumerator<object[]> GetEnumerator() => GetValues().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class MemorySourceKindData : IEnumerable<object[]>
{
    private static IEnumerable<object[]> GetValues()
    {
        Array values = Enum.GetValues(typeof(MemorySources.MemorySourceKind));
        foreach (object? value in values)
        {
            // ExternalMemory is only supported on Windows.
            bool windowsOrLinux = Polyfills.IsWindows() || Polyfills.IsLinux();
            if ((MemorySources.MemorySourceKind)value! == MemorySources.MemorySourceKind.EXTERNAL_PROCESS &&
                !windowsOrLinux)
                continue;

            yield return new[] { value };
        }
    }

    public IEnumerator<object[]> GetEnumerator() => GetValues().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class MemoryMemorySource : TemporaryMemorySource<Memory.Memory>
{
    public override Memory.Memory Memory { get; } = new();
}

public class ExternalMemoryMemorySource : TemporaryMemorySource<ExternalMemory>
{
    public override ExternalMemory Memory { get; }

    // Create dummy HelloWorld.exe
    private readonly Process _helloWorldProcess;

    public ExternalMemoryMemorySource()
    {
        var filePath = Path.GetFullPath(Assets.GetHelloWorldExePath());
        if (!Polyfills.IsWindows())
        {
            try
            {
                // Grant execute permissions to the native executable
                Process.Start(new ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"+x \"{filePath}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                })!.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        try
        {
            _helloWorldProcess = Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                CreateNoWindow = true,
                UseShellExecute = false
            })!;
        }
        catch (Win32Exception)
        {
            throw new Exception($"Failed to start process with path: {filePath}");
        }


#pragma warning disable CA1416
        Memory = new ExternalMemory(_helloWorldProcess);
#pragma warning restore CA1416
    }

    // Dispose of HelloWorld.exe
    public override void Dispose()
    {
        _helloWorldProcess.Kill();
        _helloWorldProcess.Dispose();
    }
}

public abstract class TemporaryMemorySource<T> : ITemporaryMemorySource
    where T : ICanChangeMemoryProtection, ICanReadWriteMemory, ICanAllocateMemory
{
    public abstract T Memory { get; }

    public ICanChangeMemoryProtection ChangeMemoryProtection => Memory;
    public ICanReadWriteMemory ReadWriteMemory => Memory;
    public ICanAllocateMemory AllocateMemory => Memory;

    public virtual void Dispose() => GC.SuppressFinalize(this);
}

public interface ITemporaryMemorySource : IDisposable
{
    ICanChangeMemoryProtection ChangeMemoryProtection { get; }
    ICanReadWriteMemory ReadWriteMemory { get; }
    ICanAllocateMemory AllocateMemory { get; }
}
