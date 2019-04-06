using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Position
    {
        public Position(Symbol symbol, Amount value)
        {
            Symbol = symbol;
            Value = value;
        }

        public Symbol Symbol { get; }
        public Amount Value { get; }
    }
}
