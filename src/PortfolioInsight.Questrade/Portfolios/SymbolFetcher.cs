using PortfolioInsight.Brokerages;
using PortfolioInsight.Connections;
using PortfolioInsight.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class SymbolFetcher : ISymbolFetcher
    {
        public SymbolFetcher()
        {
        }

        public async Task<NewSymbol> FetchByNameAsync(string name, AccessToken accessToken) =>
            (await SymbolApi
                .FindSymbolAsync(name, accessToken))
            ?.ToNewSymbol(Brokerage.Questrade.Id);
    }
}
