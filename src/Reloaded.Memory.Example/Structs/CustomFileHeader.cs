using System.Runtime.InteropServices;

namespace Reloaded.Memory.Example.Structs
{
    /// <summary>
    /// Defines the file name of an individual file inside a .ONE archive.
    /// </summary>
    public struct CustomFileHeader
    {
        /// <summary>
        /// Contains the actual filename of the file as ASCII encoded bytes.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;

        public uint Offset;
        public uint Length;
    }
}
