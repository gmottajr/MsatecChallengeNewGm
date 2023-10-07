using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Core.Exceptions
{
    public class ApplicationDataNotFoundException : Exception
    {
        public ApplicationDataNotFoundException()
        {
        }

        public ApplicationDataNotFoundException(string? message) : base(message)
        {
        }

        public ApplicationDataNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ApplicationDataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
