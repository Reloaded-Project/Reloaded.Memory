using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Reloaded.Memory.Exceptions
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class MemoryAllocationException : Exception
    {
        /// <inheritdoc />
        public MemoryAllocationException()
        { }

        /// <inheritdoc />
        public MemoryAllocationException(string message) : base(message)
        { }

        /// <inheritdoc />
        public MemoryAllocationException(string message, Exception innerException) : base(message, innerException)
        { }

        /// <inheritdoc />
        protected MemoryAllocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
