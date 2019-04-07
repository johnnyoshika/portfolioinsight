using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Balance
    {
        public Balance(string type, Amount value, Currency currency)
        {
            Type = type;
            Value = value;
            Currency = currency;
        }

        public string Type { get; }
        public Amount Value { get; }
        public Currency Currency { get; }
    }
}
