﻿using PortfolioInsight.Brokerages;
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
        public SymbolFetcher(ITokenizer tokenizer)
        {
            Tokenizer = tokenizer;
        }

        ITokenizer Tokenizer { get; }

        public async Task<NewSymbol> FetchByNameAsync(string name, Connection connection) =>
            (await SymbolApi
                .FindSymbolAsync(name, await Tokenizer.RefreshAsync(connection)))
            ?.ToNewSymbol(Brokerage.Questrade.Id);
    }
}
