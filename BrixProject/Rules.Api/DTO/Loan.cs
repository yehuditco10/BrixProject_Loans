using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rules.Api.DTO
{
    public class Loan
    {
        public Guid LoanId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public int ProviderId { get; set; }

    }
}
