using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class AssetClassEntity
    {
        public AssetClass ToModel() =>
            new AssetClass(
                Id,
                Name,
                (Rate?)Target);
    }
}
