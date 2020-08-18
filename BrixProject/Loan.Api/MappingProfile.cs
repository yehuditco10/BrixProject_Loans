using AutoMapper;
using Messages.Events;

namespace Loan.Api
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //לחלק ל2 קבצים אחד פה ואחד בהנדלר?
            CreateMap<DTO.Loan, Services.Models.Loan>();
            CreateMap<Data.Entities.Loan, Services.Models.Loan>();
            CreateMap<Services.Models.Loan, Data.Entities.Loan>();
            CreateMap<DTO.RulesResults, Services.Models.RulesResults>();
            CreateMap<Services.Models.Rule, Data.Entities.RuleForLoan>();
            CreateMap<DTO.Confirm, Services.Models.Confirm>();
            CreateMap<Services.Models.Loan, Messages.Commands.ValidateLoan>()
                   .ForMember(v => v.LoanId, model => model.MapFrom(loan => loan.Id));
            CreateMap<LoanValidated,Services.Models.RulesResults>();
            CreateMap<Data.Entities.RuleForLoan, Services.Models.Rule>();
            CreateMap<Services.Models.eStatus, Data.Entities.eStatus>();
        }
    }
}
