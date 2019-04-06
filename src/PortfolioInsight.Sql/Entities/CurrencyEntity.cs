using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight
{
    public partial class CurrencyEntity : Entity<CurrencyEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Code };

        public Currency ToModel() =>
            new Currency(Code, (Rate)Rate, AsOf);
    }
}
