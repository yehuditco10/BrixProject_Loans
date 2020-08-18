
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Rules.Api.Models;
using Rules.Services.Exceptions;
using Rules.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _ruleRepository;
        private readonly RulesTranslate _rulesTranslate;
        public RuleService(IRuleRepository ruleRepository,
           RulesTranslate rulesTranslate)
        {
            _ruleRepository = ruleRepository;
            _rulesTranslate = rulesTranslate;
        }

        private Func<Loan, bool> GetRuleExpression(Rule rule, Type paramType)
        {
            ParameterExpression argParam = Expression.Parameter(typeof(Loan), "loan");
            string param = _rulesTranslate.Parameters[rule.Parameter];
            Expression nameProperty = Expression.Property(argParam, param);
            ConstantExpression valueExpression;
            if (paramType.Equals(typeof(int)))
                valueExpression = Expression.Constant(int.Parse(rule.Value));
            else
                valueExpression = Expression.Constant(rule.Value);
            Expression expression;
            string sign = _rulesTranslate.Signs[rule.Condition];
            switch (sign)
            {
                case ">":
                    expression = Expression.GreaterThan(nameProperty, valueExpression);
                    break;
                case "<":
                    expression = Expression.LessThan(nameProperty, valueExpression);
                    break;
                case "=":
                    expression = Expression.Equal(nameProperty, valueExpression);
                    break;
                default:
                    expression = Expression.Equal(nameProperty, valueExpression);
                    break;
            }
            Expression<Func<Loan, bool>> lambda
                = Expression.Lambda<Func<Loan, bool>>(expression, argParam);
            return lambda.Compile();
        }

        //async?
        public async Task<Dictionary<int, bool>> ValidLoanAsync(Loan loan)
        {
            Dictionary<int, bool> rulesResults = new Dictionary<int, bool>();
            List<Rule> rules =await _ruleRepository.GetRulesByProviderIdAsync(loan.ProviderId);
            if (rules.Count() == 0)
                throw new DataNotFoundException($"There are no rules for a provider with {loan.ProviderId} id");
            foreach (var rule in rules)
            {
                var type = loan.GetType().GetProperty(_rulesTranslate.Parameters[rule.Parameter]).GetValue(loan, null).GetType();
                Func<Loan, bool> isValidRule = GetRuleExpression(rule, type);
                rulesResults.Add(rule.Id, isValidRule(loan));
            }
            return rulesResults;
        }
        private List<Rule> ReadRulesFromFile(IFormFile file, int providerId)
        {
            List<Rule> rules = new List<Rule>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            Rule rule = new Rule()
                            {
                                Parameter = reader.GetValue(0).ToString(),
                                Condition = reader.GetValue(1).ToString(),
                                Value = reader.GetValue(2).ToString(),
                                ProviderId = providerId
                            };
                            rules.Add(rule);
                        }
                    } while (reader.NextResult());
                }
            }
            return rules;
        }
        public async Task CreateRulesAsync(Microsoft.AspNetCore.Http.IFormFile file, int providerId)
        {
            await _ruleRepository.ResetRulesAsync(providerId);
            List<Rule> rules = ReadRulesFromFile(file, providerId);
            await _ruleRepository.CreateRulesAsync(rules);
        }
    }
}
