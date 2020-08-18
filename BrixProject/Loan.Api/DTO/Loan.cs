using System;

namespace Loan.Api.DTO
{
    public class Loan
    {
        public int BorrowerId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public int ProviderId { get; set; }

    }

}
