using System;
using System.Collections.Generic;
using System.Text;

namespace Likvido.ApiAuth.Common.Exceptions
{
    [Serializable]
    public class InvalidJsonFieldException : Exception
    {
        public InvalidJsonFieldException() { }
        public InvalidJsonFieldException(string message) : base(message) { }
        public InvalidJsonFieldException(string message, Exception inner) : base(message, inner) { }
        protected InvalidJsonFieldException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
