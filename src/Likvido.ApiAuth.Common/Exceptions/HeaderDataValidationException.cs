using System;
using System.Collections.Generic;
using System.Text;

namespace Likvido.ApiAuth.Common.Exceptions
{
    [Serializable]
    public class HeaderDataValidationException : Exception
    {
        public HeaderDataValidationException() { }
        public HeaderDataValidationException(string message) : base(message) { }
        public HeaderDataValidationException(string message, Exception inner) : base(message, inner) { }
        protected HeaderDataValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
