using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory
{
    public static class Endian
    {
        /// <summary>
        /// Reverses the endian of a primitive value (not struct).
        /// </summary>
        /// <returns>The endian type of a primitive value. (not struct)</returns>
        public static void Reverse<T>(ref T type, out T value, bool marshalElement) where T : unmanaged
        {
            // Declare an array for storing the data.
            byte[] data = Struct.GetBytes(ref type, marshalElement);
            Array.Reverse(data);

            // Use this base object for the storage of the value we are retrieving.
            Struct.FromArray<T>(data, out value, 0, marshalElement);
        }
    }
}
