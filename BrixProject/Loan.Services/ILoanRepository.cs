using Loan.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loan.Services
{
    public interface ILoanRepository
    {
        Task<Services.Models.Loan> CreateAsync(Models.Loan loan);
        Task CreateRulesToLoan(List<Rule> rules);
        Task ConfirmRuleAsync(Confirm confirm);
        List<Rule> GetRules(Guid loanId);
        Task UpdateLoanStatus(Guid loanId, eStatus status);
        Task<Services.Models.Loan> UpdateLoanAsync(Models.Loan loan, Guid loanId);
        Task ResetOldRulesForLoan(Guid loanId);
    }
}
