using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Financial
{
    public class Currency : ValueObject<Currency>
    {
        public Currency(string code)
        {
            Code = code.ToUpper();
        }

        public string Code { get; }

        protected override IEnumerable<object> EqualityCheckAttributes =>
            new object[] { Code };
    }
}
