using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class SymbolEntity
    {
        public Symbol ToDto() =>
            new Symbol(Id, Name, new Currency(CurrencyCode), new ListingExchange(ListingExchangeCode));

        public SymbolEntity Assign(Symbol symbol)
        {
            Name = symbol.Name;
            CurrencyCode = symbol.Currency.Code;
            ListingExchangeCode = symbol.ListingExchange.Code;
            return this;
        }
    }
}
