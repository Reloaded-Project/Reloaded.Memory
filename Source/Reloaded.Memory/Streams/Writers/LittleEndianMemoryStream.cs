using System.IO;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Writers
{
    /// <summary>
    /// A version of <see cref="EndianMemoryStream"/> that writes data in Little Endian mode.
    /// </summary>
    public partial class LittleEndianMemoryStream : EndianMemoryStream
    {
        /// <inheritdoc />
        public LittleEndianMemoryStream(ExtendedMemoryStream stream) : base(stream) { }

        /// <inheritdoc />
        public LittleEndianMemoryStream(ExtendedMemoryStream stream, bool disposeUnderlyingStream = true) : base(stream, disposeUnderlyingStream) { }

        /// <inheritdoc />
        public override void Write<T>(T[] structure) => Stream.Write(structure);

        /// <inheritdoc />
        public override void Write<T>(T structure) => Stream.Write(ref structure);

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure, bool marshalStructure) => Stream.Write(structure, marshalStructure);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure, bool marshalStructure) => Stream.Write(ref structure, marshalStructure);
    }
}
