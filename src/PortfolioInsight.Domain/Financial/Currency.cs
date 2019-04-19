using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Financial
{
    public class Currency : ValueObject<Currency>
    {
        public Currency(string code, Rate rate, DateTime asOf)
        {
            Code = code.ToUpper();
            Rate = rate;
            AsOf = asOf;
        }

        public string Code { get; }
        public Rate Rate { get; }
        public DateTime AsOf { get; }

        protected override IEnumerable<object> EqualityCheckAttributes =>
            new object[] { Code };
    }
}
