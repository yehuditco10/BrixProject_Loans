using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Data.Entities
{
    public class Loan
    {
        public Guid Id { get; set; }
        //we need it?
        public int BorrowerId { get; set; }
        public int ProviderId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public eStatus Status { get; set; }
        public string FailureReason { get; set; }
    }
 
}
