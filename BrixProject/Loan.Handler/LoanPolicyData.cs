using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Handler
{
   public class LoanPolicyData: ContainSagaData
    {
        public Guid LoanId { get; set; }
        public int BorrowerId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }
}
