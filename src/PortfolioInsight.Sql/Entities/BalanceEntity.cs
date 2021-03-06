﻿using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class BalanceEntity
    {
        public Balance ToModel() =>
            new Balance(Type, Value, Currency.ToModel());

        public BalanceEntity Assign(Balance balance)
        {
            Value = balance.Value;
            Type = balance.Type;
            CurrencyCode = balance.Currency.Code;
            return this;
        }
    }
}
