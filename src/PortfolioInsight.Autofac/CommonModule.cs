using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight
{
    public class CommonModule : Autofac.Module
    {
        public CommonModule(string connectionString)
        {
            ConnectionString = connectionString;
        }

        string ConnectionString { get; }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(_ => new Context(new DbContextOptionsBuilder<Context>().UseSqlServer(ConnectionString).Options))
                .AsSelf()
                .InstancePerDependency();

            builder
                .RegisterAssemblyOf<PortfolioInsightAutofacAssembly>()
                .RegisterAssemblyOf<PortfolioInsightDomainAssembly>()
                .RegisterAssemblyOf<PortfolioInsightSqlAssembly>()
                .RegisterAssemblyOf<PortfolioInsightQuestradeAssembly>()
                .RegisterAssemblyOf<PortfolioInsightExchangeRatesApiAssembly>()
                .RegisterAssembly(Assembly.GetEntryAssembly());
        }
    }
}
