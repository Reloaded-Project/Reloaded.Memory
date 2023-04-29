using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Reloaded.Memory.Tests.x86
{
    public class LargeAddressAwareTests
    {
        // TODO: Bring a new version of Reloaded.Memory.Buffers in.
        // once we make it large address aware using this latest Memory patch.

        #region P/Invoke
        void AssertLargeAddressAware()
        {
            var maxAddress = GetMaxAddress();
            if ((long)maxAddress <= int.MaxValue)
                Assert.False(true, "Test host is not large address aware!!");
        }

        /// <summary>
        /// Contains information about the current computer system. This includes the architecture and type of the processor, the number of
        /// processors in the system, the page size, and other such information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct SYSTEM_INFO
        {
            public int wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public nuint lpMinimumApplicationAddress;
            public nuint lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        /// <summary>
        /// <para>Retrieves information about the current system.</para>
        /// <para>To retrieve accurate information for an application running on WOW64, call the <c>GetNativeSystemInfo</c> function.</para>
        /// </summary>
        /// <param name="lpSystemInfo">A pointer to a <c>SYSTEM_INFO</c> structure that receives the information.</param>
        /// <returns>This function does not return a value.</returns>
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        /// <summary>
        /// Returns the max addressable address of the process sitting behind the <see cref="MemoryBufferHelper"/>.
        /// </summary>
        private nuint GetMaxAddress()
        {
            // Is this Windows on Windows 64? (x86 app running on x64 Windows)
            GetSystemInfo(out SYSTEM_INFO systemInfo);
            long maxAddress = 0x7FFFFFFF;

            // Check for large address aware
            if (IntPtr.Size == 4 && (uint)systemInfo.lpMaximumApplicationAddress > maxAddress)
                maxAddress = (uint)systemInfo.lpMaximumApplicationAddress;

            return (nuint)maxAddress;
        }
        #endregion
    }
}
