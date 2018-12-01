using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Reloaded.Memory.Exceptions
{
    public class PermissionChangeFailureException : Exception
    {
        public PermissionChangeFailureException()
        { }

        public PermissionChangeFailureException(string message) : base(message)
        { }

        public PermissionChangeFailureException(string message, Exception innerException) : base(message, innerException)
        { }

        protected PermissionChangeFailureException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
