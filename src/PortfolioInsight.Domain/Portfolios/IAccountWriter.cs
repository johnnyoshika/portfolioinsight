using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IAccountWriter
    {
        Task WriteAsync(int connectionId, List<Account> accounts);
    }
}
