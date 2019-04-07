using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class AssetClass
    {
        public AssetClass(int id, string name, Rate? target)
        {
            Id = id;
            Name = name;
            Target = target;
        }

        public int Id { get; }
        public string Name { get; }
        public Rate? Target { get; }
    }
}
