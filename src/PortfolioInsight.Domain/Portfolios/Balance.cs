using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Balance
    {
        public Balance(string type, Amount amount, Currency currency)
        {
            Type = type;
            Amount = amount;
            Currency = currency;
        }

        public string Type { get; }
        public Amount Amount { get; }
        public Currency Currency { get; }
    }
}
