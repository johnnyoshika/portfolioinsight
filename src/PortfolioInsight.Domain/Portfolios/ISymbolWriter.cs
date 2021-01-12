using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface ISymbolWriter
    {
        Task<Symbol> WriteAsync(NewSymbol newSymbol);
    }
}
