using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight.Reports
{
    public class Asset
    {
        public Asset(AssetClass assetClass, Amount value, Amount total)
        {
            AssetClass = assetClass;
            Value = value;
            Total = total;
        }

        public AssetClass AssetClass { get; }
        public Amount Value { get; }
        public Amount Total { get; }
        public Rate Proportion => (Rate)((decimal)Value / (decimal)Total);
        public Amount? Target => Total * AssetClass?.Target;
        public Amount? Variance => Target - Value;

        public override string ToString() => $"{AssetClass.Name} {Value} ({Proportion.Rounded}%)";
    }
}
