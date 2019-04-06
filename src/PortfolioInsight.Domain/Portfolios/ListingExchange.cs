using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class ListingExchange : ValueObject<ListingExchange>
    {
        public ListingExchange(string code)
        {
            Code = code;
        }

        public string Code { get; }

        protected override IEnumerable<object> EqualityCheckAttributes =>
            new object[] { Code };
    }
}
