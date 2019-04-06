using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Symbol
    {
        public Symbol(int id, string name, Currency currency, string listingExchange)
        {
            Id = id;
            Name = name;
            Currency = currency;
            ListingExchange = listingExchange;
        }

        public int Id { get; }
        public string Name { get; }
        public Currency Currency { get; }
        public string ListingExchange { get; }
    }
}
