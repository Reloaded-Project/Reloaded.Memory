using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Reloaded.Memory.Tests.Utilities.Structures;

/// <summary>
///     Sample struct that could represent a file entry in an archive format.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public class MarshallingStruct
{
    private static readonly Random Random = new();

    /// <summary>
    ///     An inline array of characters, would represent a file name.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string Name = null!;

    /// <summary>
    ///     The size of file after decompression.
    /// </summary>
    public int UncompressedFileSize;

    /// <summary>
    ///     The size of file compressed within the archive.
    /// </summary>
    public int CompressedSize;

    public MarshallingStruct() => Populate();

    /// <summary>
    ///     Creates a randomized instance of this struct.
    /// </summary>
    public void Populate()
    {
        Name = GetRandomString();
        CompressedSize = Random.Next();
        UncompressedFileSize = Random.Next();
    }

    /// <summary>
    ///     Get random string of 11 characters.
    /// </summary>
    public static string GetRandomString()
    {
        var path = Path.GetRandomFileName();
        path = path.Replace(".", "");
        return path;
    }

    /* Custom Equals and GetHashCode */

    public bool Equals(MarshallingStruct other) => string.Equals(Name, other.Name) &&
                                                   UncompressedFileSize == other.UncompressedFileSize &&
                                                   CompressedSize == other.CompressedSize;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is MarshallingStruct other && Equals(other);
    }

    public override int GetHashCode() => 0;
}
