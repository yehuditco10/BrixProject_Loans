using AutoMapper;
using Messages.Commands;
using Messages.Events;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using Rules.Services;
using System.Threading.Tasks;

namespace Rules.Handler
{
    public class ValidateLoanHandler : IHandleMessages<ValidateLoan>
    {
        static readonly ILog _log = LogManager.GetLogger<ValidateLoanHandler>();
        private readonly IRuleService _ruleService;
        private readonly IMapper _mapper;

        public ValidateLoanHandler(IRuleService ruleService,
            IMapper mapper)
        {
            _ruleService = ruleService;
            _mapper = mapper;
        }
        public async Task Handle(ValidateLoan message, IMessageHandlerContext context)
        {
            _log.Info("in ValidateLoanHandler");
            var res = await _ruleService.ValidLoanAsync(_mapper.Map<Api.Models.Loan>(message));
            LoanValidated loanValidated = new LoanValidated()
            {
                LoanId=message.LoanId,
                rulesResults = JsonConvert.SerializeObject(res)
            };
            _log.Info("returns the rules results");
            await context.Publish(loanValidated).ConfigureAwait(false);
        }
    }
}
