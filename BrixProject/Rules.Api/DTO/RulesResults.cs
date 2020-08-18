using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rules.Api.DTO
{
    public class RulesResults
    {
        public Guid LoanId { get; set; }
        public object rulesResults { get; set; }

     //   public Dictionary<int, bool> rulesResults { get; set; }
    }
}

