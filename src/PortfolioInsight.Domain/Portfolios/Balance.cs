using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Balance : Holding
    {
        public const string Cash = "CASH";

        public Balance(string type, Amount value, Currency currency)
            : base(value, currency)
        {
            Type = type;
        }

        public string Type { get; }
    }
}
