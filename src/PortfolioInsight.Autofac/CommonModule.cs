using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;

namespace PortfolioInsight
{
    public class CommonModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyOf<PortfolioInsightAutofacAssembly>()
                .RegisterAssemblyOf<PortfolioInsightDomainAssembly>()
                .RegisterAssemblyOf<PortfolioInsightSqlAssembly>()
                .RegisterAssembly(Assembly.GetEntryAssembly());
        }
    }
}
