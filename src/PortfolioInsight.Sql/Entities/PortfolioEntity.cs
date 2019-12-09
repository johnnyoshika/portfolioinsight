using PortfolioInsight.Portfolios;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight
{
    public partial class PortfolioEntity
    {
        public Portfolio ToModel() =>
            new Portfolio(Id, Name);
    }
}
