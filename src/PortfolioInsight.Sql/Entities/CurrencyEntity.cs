using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight
{
    public partial class CurrencyEntity
    {
        public Currency ToModel() =>
            new Currency(Code, (Rate)Rate, AsOf);
    }
}
