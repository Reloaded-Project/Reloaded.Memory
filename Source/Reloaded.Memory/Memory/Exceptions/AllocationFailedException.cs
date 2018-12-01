using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Reloaded.Memory.Exceptions
{
    public class AllocationFailedException : Exception
    {
        public AllocationFailedException()
        { }

        public AllocationFailedException(string message) : base(message)
        { }

        public AllocationFailedException(string message, Exception innerException) : base(message, innerException)
        { }

        protected AllocationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
