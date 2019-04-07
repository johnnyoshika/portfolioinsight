using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class AllocationProportion
    {
        public AllocationProportion(AssetClass assetClass, Rate rate)
        {
            AssetClass = assetClass;
            Rate = rate;
        }

        public AssetClass AssetClass { get; }
        public Rate Rate { get; }
    }
}
