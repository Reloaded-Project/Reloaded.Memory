

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Streams.Writers
{
    public abstract partial class EndianMemoryStream
    {

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public abstract void WriteInt16(Int16 data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public abstract void WriteUInt16(UInt16 data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public abstract void WriteInt32(Int32 data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public abstract void WriteUInt32(UInt32 data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public abstract void WriteInt64(Int64 data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public abstract void WriteUInt64(UInt64 data);

    }
}
