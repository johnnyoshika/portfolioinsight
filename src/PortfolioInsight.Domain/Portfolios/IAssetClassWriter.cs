using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public interface IAssetClassWriter
    {
        Task<AssetClass> WriteAsync(int portfolioId, string name, Rate? target);
    }
}
