using PortfolioInsight.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface ISymbolFetcher
    {
        Task<NewSymbol> FetchByNameAsync(string name, Connection connection);
    }
}
