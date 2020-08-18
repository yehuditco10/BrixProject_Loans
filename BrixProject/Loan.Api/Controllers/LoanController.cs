using System;
using System.Threading.Tasks;
using AutoMapper;
using Loan.Services;
using Loan.Services.Models;
using Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Loan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public LoanController(ILoanService loanService,
            IMapper mapper,
            IMessageSession messageSession)
        {
            _loanService = loanService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost]
        public async Task<ActionResult> Create(DTO.Loan loan)
        {
            var res = await _loanService.CreateAsync(_mapper.Map<Services.Models.Loan>(loan));
            ValidateLoan validateLoan = _mapper.Map<ValidateLoan>(res);
            await _messageSession.Send(validateLoan).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRule(DTO.Confirm confirm)
        {
            await _loanService.ConfirmRuleAsync(_mapper.Map<Confirm>(confirm));
            return Ok();
        }

        [HttpPut("{loanId}")]
        public async Task<ActionResult> Update(DTO.Loan loan, Guid loanId)
        {
            var res = await _loanService.UpdateLoanAsync(_mapper.Map<Services.Models.Loan>(loan), loanId);
            ValidateLoan validateLoan = _mapper.Map<ValidateLoan>(res);
            await _messageSession.Send(validateLoan).ConfigureAwait(false);
            return Ok();
        }
    }
}