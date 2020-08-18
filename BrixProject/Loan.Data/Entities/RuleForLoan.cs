using System;

namespace Loan.Data.Entities
{
    public class RuleForLoan
    {
        public int Id { get; set; }
        public Guid LoanId { get; set; }
        public int RuleId { get; set; }
        public bool Isvalid { get; set; }
        public string ManagerMessage { get; set; }

        //עושה 2 LOANID
        //  public virtual Loan loan { get; set; }

        // public virtual Rule rule { get; set; }
    }
}
