using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Exceptions
{
    public class SaveFailedException : Exception
    {
        public SaveFailedException()
        {
        }

        public SaveFailedException(string? message) : base(message)
        {
        }

        public SaveFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
