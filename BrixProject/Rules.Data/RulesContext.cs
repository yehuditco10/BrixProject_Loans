using Microsoft.EntityFrameworkCore;
using Rules.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Rules.Data
{
   public class RulesContext: DbContext
    {
        public DbSet<Rule> Rules { get; set; }
        public RulesContext(DbContextOptions<RulesContext> options)
: base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              //  optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["RuleConnection"]);
                optionsBuilder.UseSqlServer("Data Source = DESKTOP-1HT6NS2; Initial Catalog = Rules; Integrated Security = True");
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
