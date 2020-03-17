using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Reloaded.Memory.Streams.Writers
{
    /// <summary>
    /// A version of <see cref="EndianMemoryStream"/> that writes data in Little Endian mode.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class LittleEndianMemoryStream : EndianMemoryStream
    {
        /// <inheritdoc />
        public LittleEndianMemoryStream(ExtendedMemoryStream stream) : base(stream) { }

        /// <inheritdoc />
        public override void Write<T>(T[] structure) => Stream.Write(structure);

        /// <inheritdoc />
        public override void Write<T>(T structure) => Stream.Write(structure);

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure, bool marshalStructure = true) => Stream.Write(structure, marshalStructure);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure, bool marshalStructure = true) => Stream.Write(structure, marshalStructure);
    }
}
