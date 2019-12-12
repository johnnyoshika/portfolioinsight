using PortfolioInsight.Portfolios;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Reports
{
    public class PositionReport
    {
        public PositionReport(bool exclude, Position position)
        {
            Exclude = exclude;
            Position = position;
        }

        public bool Exclude { get; }
        public Position Position { get; }
    }
}
