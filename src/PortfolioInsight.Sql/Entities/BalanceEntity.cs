using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class BalanceEntity
    {
        public Balance ToModel() =>
            new Balance(Type, Value, new Currency(Currency.Code, (Rate)Currency.Rate));

        public BalanceEntity Assign(Balance balance)
        {
            Value = balance.Value;
            Type = balance.Type;
            CurrencyCode = balance.Currency.Code;
            return this;
        }
    }
}
