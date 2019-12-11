using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IExcludeReader
    {
        Task<Exclude> ReadByPortfolioIdAsync(int portfolioId);
    }
}
