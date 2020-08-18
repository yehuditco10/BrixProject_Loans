using System;

namespace Messages.Commands
{
   public class ValidateLoan
    {
        public Guid LoanId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public int ProviderId { get; set; }

    }
}
