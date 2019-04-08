using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Position : Holding
    {
        public Position(Symbol symbol, Amount value)
            : base(value, symbol.Currency)
        {
            Symbol = symbol;
        }

        public Symbol Symbol { get; }
    }
}
