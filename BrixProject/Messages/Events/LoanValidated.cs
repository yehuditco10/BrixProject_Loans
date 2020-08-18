using System;

namespace Messages.Events
{
    public class LoanValidated
    {
        public Guid LoanId { get; set; }
        public object rulesResults { get; set; }
    }
}
