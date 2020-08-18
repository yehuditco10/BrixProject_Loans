using System;

namespace Loan.Services.Models
{
   public class Rule
    {
        public Guid LoanId { get; set; }
        public int RuleId { get; set; }
        public bool Isvalid { get; set; }


    }
}
