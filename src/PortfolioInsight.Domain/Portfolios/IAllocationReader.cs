using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IAllocationReader
    {
        Task<List<Allocation>> ReadByUserIdAsync(int userId);
    }
}
