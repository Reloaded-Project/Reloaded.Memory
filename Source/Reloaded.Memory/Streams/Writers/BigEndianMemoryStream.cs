using System.IO;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Writers
{
    /// <summary>
    /// A version of <see cref="EndianMemoryStream"/> that writes data in Big Endian mode.
    /// </summary>
    public partial class BigEndianMemoryStream : EndianMemoryStream
    {
        /// <inheritdoc />
        public BigEndianMemoryStream(ExtendedMemoryStream stream) : base(stream) {}

        /// <inheritdoc />
        public BigEndianMemoryStream(ExtendedMemoryStream stream, bool disposeUnderlyingStream = true) : base(stream, disposeUnderlyingStream) { }

        /// <inheritdoc />
        public override void Write<T>(T[] structure) => Stream.WriteBigEndianPrimitive(structure);

        /// <inheritdoc />
        public override void Write<T>(T structure) => Stream.WriteBigEndianPrimitive(structure);

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteStruct<T>(T[] structures) where T : unmanaged, IEndianReversible => Stream.WriteBigEndianStruct(structures);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteStruct<T>(T structure) where T : unmanaged, IEndianReversible => Stream.WriteBigEndianStruct(structure);
    }
}
