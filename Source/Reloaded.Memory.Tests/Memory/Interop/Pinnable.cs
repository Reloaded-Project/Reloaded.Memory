using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Interop;
using Reloaded.Memory.Shared.Structs;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Interop
{
    public class Pinnable
    {
        [Fact]
        public unsafe void Pin()
        {
            var pinnable = new Pinnable<int>(0);
            for (int x = 0; x < 100; x++)
            {
                pinnable.Value = x;
                Assert.Equal(pinnable.Value, *pinnable.Pointer);
            }

            pinnable.Dispose();
        }

        // Pinnable<T> should not dispose native object, only PinnableDisposable<T>.
        [Fact]
        public void NoDispose()
        {
            var pinnable = new Pinnable<DisposeChecker>(new DisposeChecker());
            pinnable.Dispose();

            Assert.Equal(0, pinnable.Value.Disposed);
            pinnable.Value.Dispose();

            Assert.Equal(1, pinnable.Value.Disposed);
        }

        [Fact]
        public void Dispose()
        {
            var pinnable = new PinnableDisposable<DisposeChecker>(new DisposeChecker());
            pinnable.Dispose();

            Assert.Equal(1, pinnable.Value.Disposed);
        }
    }
}
