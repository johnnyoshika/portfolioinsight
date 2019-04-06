using PortfolioInsight.Portfolios;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight
{
    public partial class PortfolioEntity : Entity<PortfolioEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

        public Portfolio ToModel() =>
            new Portfolio(Id, Name);
    }
}
