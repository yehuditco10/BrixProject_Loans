using Loan.Services.Models;
using System;
using System.Threading.Tasks;

namespace Loan.Services
{
    public interface ILoanService
    {
        Task<Models.Loan> CreateAsync(Models.Loan loan);
        Task CreateRulesToLoan(RulesResults rulesResults);
        Task ConfirmRuleAsync(Confirm confirm);
        Task UpdateLoanStatusAsync(Guid loanId);
        Task<Models.Loan> UpdateLoanAsync(Models.Loan loan, Guid loanId);
        Task ResetOldRulesForLoan(Guid loanId);
    }
}
