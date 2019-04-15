using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IAssetClassReader
    {
        Task<AssetClass> ReadByIdAsync(int id);
        Task<AssetClass> ReadByNameAsync(int userId, string name);
        Task<AssetClass> ReadCashByUserIdAsync(int userId);
    }
}
