using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory
{
    public static class Endian
    {
        /// <summary>
        /// Reverses the endian of a primitive value such as int, short, float, double etc. (Not including structs).
        /// </summary>
        /// <param name="type">The individual value to be byte reversed.</param>
        /// <param name="swapped">The output variable to receive the swapped out value.</param>
        public static void Reverse<T>(ref T type, out T swapped) where T : unmanaged
        {
            // Declare an array for storing the data.
            byte[] data = Struct.GetBytes(ref type);
            Array.Reverse(data);

            // Use this base object for the storage of the value we are retrieving.
            Struct.FromArray<T>(data, out swapped);
        }
    }
}
