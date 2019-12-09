using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IPortfolioReader
    {
        Task<Portfolio> ReadByIdAsync(int id);
        Task<List<Portfolio>> ReadByUserIdAsync(int userId);

        Task<bool> UserOwnsPortfolio(int portfolioId, int userId);
    }
}
