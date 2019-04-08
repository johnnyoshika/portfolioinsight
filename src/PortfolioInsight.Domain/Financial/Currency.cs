using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Financial
{
    public class Currency : ValueObject<Currency>
    {
        public Currency(string code, Rate rate)
        {
            Code = code.ToUpper();
            Rate = rate;
        }

        public string Code { get; }
        public Rate Rate { get; }

        protected override IEnumerable<object> EqualityCheckAttributes =>
            new object[] { Code };
    }
}
