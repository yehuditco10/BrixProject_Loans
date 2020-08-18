using AutoMapper;
using Loan.Services;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Handler
{
    public class LoanPolicySaga : Saga<LoanPolicyData>,
         IAmStartedByMessages<ValidateLoan>,
        IHandleMessages<LoanValidated>
    {
        static readonly ILog _log = LogManager.GetLogger<LoanPolicySaga>();
        private readonly IMapper _mapper;
        private readonly ILoanService _loanService;

        public LoanPolicySaga(IMapper mapper,ILoanService loanService)
        {
            _mapper = mapper;
            _loanService = loanService;
        }
        public async Task Handle(ValidateLoan message, IMessageHandlerContext context)
        {
            _log.Info("in LoanSaga");
            await context.Send(message).ConfigureAwait(false);
        }

        public async Task Handle(LoanValidated message, IMessageHandlerContext context)
        {
            await _loanService.ResetOldRulesForLoan(message.LoanId);
            await _loanService.CreateRulesToLoan(_mapper.Map<Services.Models.RulesResults>(message));
            await _loanService.UpdateLoanStatusAsync(message.LoanId);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<LoanPolicyData> mapper)
        {
            mapper.ConfigureMapping<ValidateLoan>(message => message.LoanId)
                                   .ToSaga(sagaData => sagaData.LoanId);
            mapper.ConfigureMapping<LoanValidated>(message => message.LoanId)
                                  .ToSaga(sagaData => sagaData.LoanId);
        }
    }
}
