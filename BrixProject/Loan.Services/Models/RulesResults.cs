using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Services.Models
{
    public class RulesResults
    {
        public Guid LoanId { get; set; }
        public string rulesResults { get; set; }

        //   public Dictionary<int, bool> rulesResults { get; set; }
    }
}
