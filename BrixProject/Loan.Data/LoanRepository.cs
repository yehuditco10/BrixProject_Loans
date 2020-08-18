using AutoMapper;
using Loan.Services;
using Loan.Services.Exceptions;
using Loan.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan.Data
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LoanContext _loanContext;
        private readonly IMapper _mapper;

        public LoanRepository(LoanContext loanContext,
            IMapper mapper)
        {
            _loanContext = loanContext;
            _mapper = mapper;
        }

        public async Task ConfirmRuleAsync(Confirm confirm)
        {
            Entities.RuleForLoan ruleForLoan = await _loanContext.RulesForLoan
                .FirstOrDefaultAsync(l => l.LoanId == confirm.LoanId && l.RuleId == confirm.RuleId);
            if (ruleForLoan == null)
                throw new DataNotFoundException($"There are no rules for a loan with {confirm.LoanId} id");
            ruleForLoan.Isvalid = true;
            ruleForLoan.ManagerMessage = confirm.MenamgerMessage;
            await _loanContext.SaveChangesAsync();
        }

        public async Task<Services.Models.Loan> CreateAsync(Services.Models.Loan loan)
        {
            Entities.Loan newLoan = _mapper.Map<Entities.Loan>(loan);
            newLoan.Id = Guid.NewGuid();
            newLoan.Status = Entities.eStatus.processing;
            await _loanContext.AddAsync(newLoan);
            await _loanContext.SaveChangesAsync();
            return _mapper.Map<Services.Models.Loan>(newLoan);
        }

        public List<Rule> GetRules(Guid loanId)
        {
            var rules = _loanContext.RulesForLoan.Where(r => r.LoanId == loanId).ToList();
            return _mapper.Map<List<Rule>>(rules);
        }

        public async Task ResetOldRulesForLoan(Guid loanId)
        {
            var oldlist = _loanContext.RulesForLoan.Where(r => r.LoanId == loanId).ToList();
            if (oldlist.Count() > 0)
            {
                _loanContext.RulesForLoan.RemoveRange(oldlist);
                await _loanContext.SaveChangesAsync();
            }
        }

        public async Task<Services.Models.Loan> UpdateLoanAsync(Services.Models.Loan loan, Guid loanId)
        {
            var loanToUpdate = await _loanContext.Loans.FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null)
                throw new DataNotFoundException($"There is no loan with {loanId} ID");
            loanToUpdate.ProviderId = loan.ProviderId;
            loanToUpdate.Age = loan.Age;
            loanToUpdate.City = loan.City;
            loanToUpdate.Balance = loan.Balance;
            loanToUpdate.Amount = loan.Amount;
            await _loanContext.SaveChangesAsync();
            return _mapper.Map<Services.Models.Loan>(loanToUpdate);
        }

        public async Task UpdateLoanStatus(Guid loanId, eStatus status)
        {
            var loan = await _loanContext.Loans.FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null)
                throw new DataNotFoundException($"There is no loan with {loanId} ID");
            loan.Status = _mapper.Map<Entities.eStatus>(status);
            await _loanContext.SaveChangesAsync();
        }
        public async Task CreateRulesToLoan(List<Rule> rules)
        {
            _loanContext.RulesForLoan.AddRange(_mapper.Map<List<Entities.RuleForLoan>>(rules));
            await _loanContext.SaveChangesAsync();
        }
    }
}
