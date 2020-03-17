using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Readers
{
    /// <summary>
    /// A version of <see cref="EndianStreamReader"/> that reads data in Little Endian mode.
    /// </summary>
    public partial class LittleEndianStreamReader : EndianStreamReader
    {
        /// <summary>
        /// Constructs a <see cref="EndianStreamReader"/> given an existing stream reader.
        /// </summary>
        public LittleEndianStreamReader(BufferedStreamReader streamReader) : base(streamReader) { }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read<T>(out T value) => Reader.Read(out value);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe T Read<T>() => Reader.Read<T>();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek<T>(out T value) => Reader.Peek(out value);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe T Peek<T>() => Reader.Peek<T>();

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="value">The value to output.</param>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<T>(out T value, bool marshal) => Reader.Read(out value, marshal);

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>(bool marshal) => Reader.Read<T>(marshal);

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream without incrementing the position.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="value">The value to output.</param>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Peek<T>(out T value, bool marshal) => Reader.Peek(out value, marshal);

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream without incrementing the position.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek<T>(bool marshal) => Reader.Peek<T>(marshal);
    }
}
