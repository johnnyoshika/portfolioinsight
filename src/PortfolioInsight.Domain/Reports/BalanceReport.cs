using PortfolioInsight.Portfolios;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Reports
{
    public class BalanceReport
    {
        public BalanceReport(bool exclude, Balance balance)
        {
            Exclude = exclude;
            Balance = balance;
        }

        public bool Exclude { get; }
        public Balance Balance { get; }
    }
}
