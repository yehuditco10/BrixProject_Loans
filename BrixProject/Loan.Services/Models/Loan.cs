using System;

namespace Loan.Services.Models
{
   public class Loan
    {
        public Guid Id { get; set; }
        public int BorrowerId { get; set; }
        public int ProviderId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }
}
