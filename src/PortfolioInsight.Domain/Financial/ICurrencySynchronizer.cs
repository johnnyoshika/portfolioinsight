using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Financial
{
    public interface ICurrencySynchronizer
    {
        Task SyncAsync();
    }
}
