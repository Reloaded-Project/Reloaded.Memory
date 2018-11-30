using System;
using Reloaded.Memory.Sources;
using Xunit;

namespace Reloaded.Memory.Tests
{
    public class ThisProcess
    {
        /// <summary>
        /// The memory source to which we will read/write from/to.
        /// </summary>
        private IMemory _source = new Sources.Memory();

        /// <summary>
        /// Attempts to allocate memory, checks if result points to valid memory.
        /// </summary>
        [Fact]
        public void TestAllocateMemory()
        {
            IntPtr pointer = _source.Allocate(0xFFFF);
            Assert.NotEqual((IntPtr)0, pointer);
            _source.Free(pointer);
        }
    }
}
