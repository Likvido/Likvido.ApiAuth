using System;
using System.Collections.Generic;
using System.Text;

namespace Likvido.ApiAuth.Common.Exceptions
{
    [Serializable]
    public class MissingJsonFieldException : Exception
    {
        public MissingJsonFieldException() { }
        public MissingJsonFieldException(string message) : base(message) { }
        public MissingJsonFieldException(string message, Exception inner) : base(message, inner) { }
        protected MissingJsonFieldException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
