using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rules.Services;
using Rules.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rules.Data
{
    public class RuleRepository : IRuleRepository
    {
        private readonly RulesContext _rulesContext;
        private readonly IMapper _mapper;

        public RuleRepository(RulesContext rulesContext,
            IMapper mapper)
        {
            _rulesContext = rulesContext;
            _mapper = mapper;
        }
       
        public async Task<List<Rule>> GetRulesByProviderIdAsync(int providerId)
        {
            List<Entities.Rule> rules = await _rulesContext.Rules
                        .Where(r => r.ProviderId == providerId)
                        .ToListAsync(); 
            return _mapper.Map<List<Rule>>(rules);
        }

        public async Task CreateRulesAsync(List<Rule> rules)
        {
            var newRules = _mapper.Map<List<Entities.Rule>>(rules);
            await _rulesContext.AddRangeAsync(newRules);
            await _rulesContext.SaveChangesAsync();
        }

        public async Task ResetRulesAsync(int providerId)
        {
            var rules = _rulesContext.Rules.Where(r => r.ProviderId == providerId).ToListAsync();
            if (rules.Result.Count() > 0)
            {
                _rulesContext.Rules.RemoveRange(rules.Result);
                await _rulesContext.SaveChangesAsync();
            }
        }
    }
}
