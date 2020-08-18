using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Loan.Data.Entities;

namespace Loan.Data
{
   public class LoanContext:DbContext
    {
        public DbSet<Entities.Loan> Loans { get; set; }
        public DbSet<RuleForLoan> RulesForLoan { get; set; }
        
        public LoanContext(DbContextOptions<LoanContext> options)
: base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["LoanConnection"]);
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
