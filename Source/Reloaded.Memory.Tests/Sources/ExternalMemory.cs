using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Tests.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Sources
{
    public class ExternalMemory
    {
        /// <summary>
        /// Attempts to write data to an invalid address of an external process.
        /// </summary>
        [Fact]
        public void ReadWriteFail()
        {
            // Prepare
            Process helloWorldProcess = Process.Start("HelloWorld.exe");
            Memory.Sources.ExternalMemory externalMemory = new Memory.Sources.ExternalMemory(helloWorldProcess);

            /* Start Test */
            Assert.Throws<MemoryException>(() => externalMemory.Read((IntPtr)(-1), out int _));
            Assert.Throws<MemoryException>(() => externalMemory.ReadRaw((IntPtr)(-1), out byte[] _, 100));

            Assert.Throws<MemoryException>(() =>
            {
                int number = 5;
                externalMemory.Write((IntPtr) (-1), ref number);
            });

            Assert.Throws<MemoryException>(() => externalMemory.WriteRaw((IntPtr)(-1), new byte[10]));

            /* End Test */
            helloWorldProcess.Kill();
            helloWorldProcess.Dispose();
        }
    }
}
