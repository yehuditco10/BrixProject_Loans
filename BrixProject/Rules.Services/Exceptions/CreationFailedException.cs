using System;

namespace Rules.Services.Exceptions
{
    public class CreationFailedException : Exception
    {
        public CreationFailedException()
        {

        }
        public CreationFailedException(string message)
            : base(message)
        {

        }
        public CreationFailedException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
