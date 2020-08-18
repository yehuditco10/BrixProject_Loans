using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Services.Models
{
    public class Confirm
    {
        public Guid LoanId { get; set; }
        public int RuleId { get; set; }
        public string MenamgerMessage { get; set; }

    }
}
