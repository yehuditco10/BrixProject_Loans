using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Handler
{
   public class LoanPolicyData: ContainSagaData
    {
        public Guid LoanId { get; set; }
    }
}
