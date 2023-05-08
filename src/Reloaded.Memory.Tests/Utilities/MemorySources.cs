using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

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
        foreach (var value in values)
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

public class MemoryMemorySource : TemporaryMemorySource<Memory>
{
    public override Memory Memory { get; } = new();
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
                FileName = filePath, CreateNoWindow = true, UseShellExecute = false
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
    public void SafeWrite(nuint memoryAddress, Span<byte> data) => Memory.SafeRead(memoryAddress, data);
    public void SafeRead(nuint memoryAddress, Span<byte> data) => Memory.SafeWrite(memoryAddress, data);

    public virtual void Dispose() => GC.SuppressFinalize(this);
}

public interface ITemporaryMemorySource : IDisposable
{
    ICanChangeMemoryProtection ChangeMemoryProtection { get; }
    ICanReadWriteMemory ReadWriteMemory { get; }
    ICanAllocateMemory AllocateMemory { get; }
    void SafeWrite(nuint memoryAddress, Span<byte> data);
    void SafeRead(nuint memoryAddress, Span<byte> data);
}
