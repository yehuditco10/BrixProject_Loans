using System;

namespace Loan.Api.DTO
{
    public class RulesResults
    {
        public Guid LoanId { get; set; }
        public object rulesResults { get; set; }

     //   public Dictionary<int, bool> rulesResults { get; set; }
    }
}
