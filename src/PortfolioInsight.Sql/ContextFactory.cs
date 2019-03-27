using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PortfolioInsight
{
    /// <summary>
    /// Used to inject connection string in Context for EF migrations.
    /// From package manager console, the following 2 commands will look for an implementing class of IDesignTimeDbContextFactory
    /// and use the Context (with the connection string) that is creates.
    /// 
    /// $ add-migration Migration_Name
    /// $ update-database
    /// 
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation#from-a-design-time-factory
    /// </summary>
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer("Server=.;Database=PortfolioInsight;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new Context(optionsBuilder.Options);
        }
    }
}
