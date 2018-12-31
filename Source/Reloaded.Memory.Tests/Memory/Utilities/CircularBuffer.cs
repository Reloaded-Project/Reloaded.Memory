using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Utilities
{
    public class CircularBuffer
    {

        /// <summary>
        /// Extreme test which just about only fills the buffer, tests
        /// whether the buffer looped, then loops the buffer and validates
        /// pointer reset.
        ///
        /// Because <see cref="Reloaded.Memory.Utilities.CircularBuffer"/> is fairly small, it also tests
        /// all other functionality of the buffer along the way.
        /// </summary>
        [Fact]
        public void TestBuffer()
        {
            // Setup
            int bufferSize = 4096;
            var memory = new Reloaded.Memory.Sources.Memory();
            var buffer = new Reloaded.Memory.Utilities.CircularBuffer(bufferSize, memory);

            // Properties.
            int structSize     = Struct.GetSize<RandomIntStruct>();
            int bufferElements = bufferSize / structSize;

            var randomIntStructs = new RandomIntStruct[bufferElements];
            for (int x = 0; x < bufferElements; x++)
                randomIntStructs[x] = RandomIntStruct.BuildRandomStruct();

            // Fill circular buffer and validate.
            for (int x = 0; x < bufferElements; x++)
            {
                // Save write pointer.
                IntPtr nextPointer = buffer.WritePointer + structSize;

                var address = buffer.Add(ref randomIntStructs[x]);
                memory.Read(address, out RandomIntStruct randIntStruct);

                // Check correct element and pointer did not loop.
                Assert.Equal(randomIntStructs[x], randIntStruct);
                Assert.Equal(nextPointer, buffer.WritePointer);
            }

            // Now check if item can fit, it should not.
            // Buffer should require looping.
            var randomIntStruct = RandomIntStruct.BuildRandomStruct();
            var canFit = buffer.CanItemFit(ref randomIntStruct);

            Assert.Equal(Reloaded.Memory.Utilities.CircularBuffer.ItemFit.StartOfBuffer, canFit);

            // Add item to loop.
            var lastItemPtr = buffer.Add(ref randomIntStruct);
            Assert.Equal(buffer.Address, lastItemPtr); // New item should have been placed at offset 0.

            // Item too big.
            buffer.Offset = 0;
            byte[] sameSizeAsBuffer = new byte[bufferSize];
            byte[] tooBigForBuffer = new byte[bufferSize + 1];

            // Will not fit.
            Assert.Equal(Reloaded.Memory.Utilities.CircularBuffer.ItemFit.No, buffer.CanItemFit(tooBigForBuffer.Length));
            Assert.Equal(IntPtr.Zero, buffer.Add(tooBigForBuffer));

            // Will fit in current loop. Note Offset = 0.
            Assert.Equal(Reloaded.Memory.Utilities.CircularBuffer.ItemFit.Yes, buffer.CanItemFit(sameSizeAsBuffer.Length));
            Assert.NotEqual(IntPtr.Zero, buffer.Add(sameSizeAsBuffer));
            
            buffer.Offset = 1; // Will fit on next loop.
            Assert.Equal(Reloaded.Memory.Utilities.CircularBuffer.ItemFit.StartOfBuffer, buffer.CanItemFit(sameSizeAsBuffer.Length));
            Assert.NotEqual(IntPtr.Zero, buffer.Add(sameSizeAsBuffer));
            
            // Cleanup
            buffer.Dispose();
        }
    }
}
