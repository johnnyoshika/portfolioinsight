using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Authorizations;

namespace PortfolioInsight.Portfolios
{
    public interface IPortfolioSynchronizer
    {
        Task SyncAsync(Authorization authorization);
    }
}
