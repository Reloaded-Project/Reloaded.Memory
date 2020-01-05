using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Reloaded.Memory.Shared.Structs
{
    /// <summary>
    /// Sample struct that could represent a file entry in an archive format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MarshallingStruct
    {
        public static Random Random = new Random();

        /// <summary>
        /// An inline array of characters, would represent a file name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;

        /// <summary>
        /// The size of file after decompression.
        /// </summary>
        public int UncompressedFileSize;

        /// <summary>
        /// The size of file compressed within the archive.
        /// </summary>
        public int CompressedSize;


        /// <summary>
        /// Creates a randomized instance of this struct.
        /// </summary>
        /// <returns></returns>
        public static MarshallingStruct BuildRandomStruct()
        {
            MarshallingStruct marshallingStruct;
            marshallingStruct.Name = GetRandomString();
            marshallingStruct.CompressedSize = Random.Next();
            marshallingStruct.UncompressedFileSize = Random.Next();
            return marshallingStruct;
        }

        /// <summary>
        /// Get random string of 11 characters.
        /// </summary>
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

        /* Custom Equals and GetHashCode */

        public bool Equals(MarshallingStruct other)
        {
            return string.Equals(Name, other.Name) &&
                   UncompressedFileSize == other.UncompressedFileSize &&
                   CompressedSize == other.CompressedSize;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is MarshallingStruct other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ UncompressedFileSize;
                hashCode = (hashCode * 397) ^ CompressedSize;
                return hashCode;
            }
        }
    }
}
