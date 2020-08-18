using Rules.Api.Models;
using Rules.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Services
{
    public interface IRuleService
    {
        Task<Dictionary<int, bool>> ValidLoanAsync(Loan loan);
        Task CreateRulesAsync(Microsoft.AspNetCore.Http.IFormFile file, int providerId);
    }
}
