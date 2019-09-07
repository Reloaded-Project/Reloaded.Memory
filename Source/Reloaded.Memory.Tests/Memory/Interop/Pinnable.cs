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

        [Fact]
        public unsafe void PinArray()
        {
            int[] integers = { 0, 1, 2, 3, 4, 5, 6 };
            var pinnable = new Pinnable<int>(integers);

            // Integers are on the heap, should be pinned by reference.
            for (int x = 0; x < integers.Length; x++)
            {
                integers[x] = 100 + x;
                Assert.Equal(integers[x], pinnable.Pointer[x]);
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
