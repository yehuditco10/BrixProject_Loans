
using Rules.Services.Models;
using System.Collections.Generic;
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
