using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Reloaded.Memory.Exceptions
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class MemoryPermissionException : Exception
    {
        /// <inheritdoc />
        public MemoryPermissionException()
        { }

        /// <inheritdoc />
        public MemoryPermissionException(string message) : base(message)
        { }

        /// <inheritdoc />
        public MemoryPermissionException(string message, Exception innerException) : base(message, innerException)
        { }

        /// <inheritdoc />
        protected MemoryPermissionException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
