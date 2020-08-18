using Loan.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loan.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task ConfirmRuleAsync(Confirm confirm)
        {
            await _loanRepository.ConfirmRuleAsync(confirm);
            await UpdateLoanStatusAsync(confirm.LoanId);
        }

        public async Task<Models.Loan> CreateAsync(Models.Loan loan)
        {
           return await _loanRepository.CreateAsync(loan);
        }
        public async Task CreateRulesToLoan(RulesResults rulesResults)
        {
            Dictionary<int, bool> values =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(rulesResults.rulesResults);
            List<Rule> rules = new List<Rule>();
            foreach (var rule in values)
            {
                rules.Add(new Rule()
                {
                    LoanId = rulesResults.LoanId,
                    RuleId = rule.Key,
                    Isvalid = rule.Value
                });
            }
            await _loanRepository.CreateRulesToLoan(rules);
        }

        public async Task ResetOldRulesForLoan(Guid loanId)
        {
            await _loanRepository.ResetOldRulesForLoan(loanId);
        }

        public async Task<Models.Loan> UpdateLoanAsync(Models.Loan loan, Guid loanId)
        {
            var updatedLoan = await _loanRepository.UpdateLoanAsync(loan, loanId);
            return updatedLoan;
        }

        public async Task UpdateLoanStatusAsync(Guid loanId)
        {
            List<Rule> rules = _loanRepository.GetRules(loanId);
            bool isFailed = false;
            foreach (var rule in rules)
            {
                if (rule.Isvalid == false)
                {
                    await _loanRepository.UpdateLoanStatus(loanId, eStatus.failed);
                    isFailed = true;
                    break;
                }
            }
            if (isFailed == false)
                await _loanRepository.UpdateLoanStatus(loanId, eStatus.successed);
        }
    }
}
