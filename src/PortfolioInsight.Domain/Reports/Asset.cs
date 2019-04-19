using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight.Reports
{
    public class Asset
    {
        public Asset(AssetClass assetClass, Amount value, Rate proportion)
        {
            AssetClass = assetClass;
            Value = value;
            Proportion = proportion;
        }

        public AssetClass AssetClass { get; }
        public Amount Value { get; }
        public Rate Proportion { get; }
        public Amount? Target => Value / Proportion * AssetClass?.Target;
        public Amount? Variance => Target - Value;

        public override string ToString() => $"{AssetClass.Name} {Value} ({Proportion.Rounded}%)";
    }
}
