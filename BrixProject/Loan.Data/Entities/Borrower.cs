using System;
using System.Collections.Generic;
using System.Text;

namespace Loan.Data.Entities
{
   public class Borrower
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public int Age { get; set; }
        public string  City { get; set; }
    }
}
