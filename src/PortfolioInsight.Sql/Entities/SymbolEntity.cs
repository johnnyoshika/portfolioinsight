using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class SymbolEntity : Entity<SymbolEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

        public Symbol ToModel() =>
            new Symbol(
                Id,
                Name,
                Description,
                Currency.ToModel(),
                ListingExchangeCode == null ? Portfolios.ListingExchange.None : new ListingExchange(ListingExchangeCode));

        public SymbolEntity Assign(Symbol symbol)
        {
            Name = symbol.Name;
            Description = symbol.Description;
            CurrencyCode = symbol.Currency.Code;
            ListingExchangeCode = symbol.ListingExchange.Code;
            return this;
        }
    }
}
