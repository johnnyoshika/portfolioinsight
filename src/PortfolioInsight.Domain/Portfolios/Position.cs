using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Position
    {
        public Position(Symbol symbol, Amount value, Currency currency)
        {
            Symbol = symbol;
            Value = value;
            Currency = currency;
        }

        public Symbol Symbol { get; }
        public Amount Value { get; }
        public Currency Currency { get; }
    }
}
