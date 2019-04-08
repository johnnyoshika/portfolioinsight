using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public abstract class Holding
    {
        public Holding(Amount value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        public Amount Value { get; }
        public Currency Currency { get; }

        public Amount BaseValue =>
            Value * Currency.Rate;

        public Amount ValueIn(Currency currency) =>
            BaseValue / currency.Rate;
    }
}
