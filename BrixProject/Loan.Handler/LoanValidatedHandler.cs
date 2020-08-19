using AutoMapper;
using Loan.Services;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Loan.Handler
{
    public class LoanValidatedHandler : IHandleMessages<LoanValidated>
    {
        static readonly ILog _log = LogManager.GetLogger<LoanValidatedHandler>();
        private readonly IMapper _mapper;
        private readonly ILoanService _loanService;
        public LoanValidatedHandler(IMapper mapper, ILoanService loanService)
        {
            _mapper = mapper;
            _loanService = loanService;
        }
        public async Task Handle(LoanValidated message, IMessageHandlerContext context)
        {
            _log.Info("recived results from rules service");
            await _loanService.ResetOldRulesForLoan(message.LoanId);
            await _loanService.CreateRulesToLoan(_mapper.Map<Services.Models.RulesResults>(message));
            await _loanService.UpdateLoanStatusAsync(message.LoanId);
        }
    }
}
