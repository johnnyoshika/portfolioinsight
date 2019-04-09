using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Symbol : ValueObject<Symbol>
    {
        public Symbol(int id, string name, string description, Currency currency, ListingExchange listingExchange)
        {
            Id = id;
            Name = name;
            Description = description;
            Currency = currency;
            ListingExchange = listingExchange;
        }

        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Currency Currency { get; }
        public ListingExchange ListingExchange { get; }

        protected override IEnumerable<object> EqualityCheckAttributes =>
            new object[] { Id, Name, Description, Currency, ListingExchange };
    }
}
