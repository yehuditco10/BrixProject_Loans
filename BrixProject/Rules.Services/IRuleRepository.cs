using Microsoft.AspNetCore.Http;
using Rules.Api.Models;
using Rules.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Services
{
    public interface IRuleRepository
    {

        Task<List<Rule>> GetRulesByProviderIdAsync(int providerId);
        Task ResetRulesAsync(int providerId);
        Task CreateRulesAsync(List<Rule> rules);
    }
}
