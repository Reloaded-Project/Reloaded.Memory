using System;
using System.Runtime.InteropServices;

namespace Reloaded.Memory.Shared.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisposeChecker : IDisposable
    {
        public int Disposed;
        public void Dispose()
        {
            Disposed = 1;
        }
    }
}