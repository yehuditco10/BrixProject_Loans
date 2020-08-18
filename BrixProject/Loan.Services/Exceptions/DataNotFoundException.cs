using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Services.Exceptions
{
   public class DataNotFoundException:Exception
    {
        public DataNotFoundException()
        {
        }

        public DataNotFoundException(string message)
            : base(message)
        {
        }

        public DataNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
