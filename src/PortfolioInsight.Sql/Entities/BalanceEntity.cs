using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class BalanceEntity
    {
        public BalanceEntity Assign(Balance balance)
        {
            Value = balance.Amount;
            Type = balance.Type;
            CurrencyCode = balance.Currency.Code;
            return this;
        }
    }
}
