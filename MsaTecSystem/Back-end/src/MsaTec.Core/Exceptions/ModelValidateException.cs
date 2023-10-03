using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Core.Exceptions
{
    public class ModelValidateException : Exception
    {
        public ModelValidateException()
        {
        }

        public ModelValidateException(string? message) : base(message)
        {
        }

        public ModelValidateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ModelValidateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
