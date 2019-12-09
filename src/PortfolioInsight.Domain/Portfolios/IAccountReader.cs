using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IAccountReader
    {
        Task<List<Account>> ReadByConnectionIdAsync(int connectionId);
    }
}
