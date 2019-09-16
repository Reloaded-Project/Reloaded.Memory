using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory
{
    /// <summary>
    /// An interface applicable to classes or structs exposing a method to reverse the endian of all members.
    /// </summary>
    public interface IEndianReversible
    {
        /// <summary>
        /// Reverses the endian-ness of this class or struct instance.
        /// </summary>
        void SwapEndian();
    }
}
