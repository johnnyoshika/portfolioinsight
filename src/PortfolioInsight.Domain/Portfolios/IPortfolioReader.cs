using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IPortfolioReader
    {
        Task<List<Portfolio>> ReadByUserIdAsync(int userId);
    }
}
