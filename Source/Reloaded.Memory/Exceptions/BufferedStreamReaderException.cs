using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Reloaded.Memory.Exceptions
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    internal class BufferedStreamReaderException : Exception
    {
        public BufferedStreamReaderException() { }

        protected BufferedStreamReaderException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public BufferedStreamReaderException(string? message) : base(message) { }

        public BufferedStreamReaderException(string? message, Exception? innerException) : base(message, innerException) { }

        [MethodImpl(MethodImplOptions.NoInlining)] // shouldn't inline anyway because exception, but just in case
        public static void Throw(string message) => throw new BufferedStreamReaderException(message);
    }
}
