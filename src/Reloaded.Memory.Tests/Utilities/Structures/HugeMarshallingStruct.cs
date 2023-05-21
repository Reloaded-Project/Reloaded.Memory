using System.Runtime.InteropServices;

namespace Reloaded.Memory.Tests.Utilities.Structures;

/// <summary>
///     Sample struct that could represent a file entry in an archive format.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = 65536)]
public class HugeMarshallingStruct : MarshallingStruct { }
