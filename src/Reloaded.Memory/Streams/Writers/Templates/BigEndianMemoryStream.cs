

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.IO;
using Reloaded.Memory.Streams.Readers;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Streams.Writers
{
    /// <summary>
    /// A version of <see cref="EndianMemoryStream"/> that writes data in Big Endian mode.
    /// </summary>
    public partial class BigEndianMemoryStream : EndianMemoryStream
    {
    
		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteInt16(Int16 data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteUInt16(UInt16 data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteInt32(Int32 data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteUInt32(UInt32 data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteInt64(Int64 data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteUInt64(UInt64 data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteSingle(Single data) => Stream.WriteBigEndianPrimitive(data);

		/// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void WriteDouble(Double data) => Stream.WriteBigEndianPrimitive(data);


    }
}
