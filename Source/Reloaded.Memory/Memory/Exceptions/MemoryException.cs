using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Reloaded.Memory.Exceptions
{
    public class MemoryException : Exception
    {
        public MemoryException()
        { }

        public MemoryException(string message) : base(message)
        { }

        public MemoryException(string message, Exception innerException) : base(message, innerException)
        { }

        protected MemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
