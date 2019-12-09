using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Connections;

namespace PortfolioInsight.Portfolios
{
    public interface IConnectionSynchronizer
    {
        Task SyncAsync(Connection connection);
    }
}
