using AutoMapper;
using Messages.Commands;
using Rules.Services.Models;

namespace Rules.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DTO.Loan, Models.Loan>();
            CreateMap<Data.Entities.Rule, Rule>();
            CreateMap<Rule, Data.Entities.Rule>();
            CreateMap<ValidateLoan, Models.Loan>();
        }

    }
}
