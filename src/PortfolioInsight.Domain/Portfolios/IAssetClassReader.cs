using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IAssetClassReader
    {
        Task<AssetClass> ReadByIdAsync(int id);
        Task<AssetClass> ReadByNameAsync(int portfolioId, string name);
        Task<AssetClass> ReadCashByPortfolioIdAsync(int portfolioId);
    }
}
