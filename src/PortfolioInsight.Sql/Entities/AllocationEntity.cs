using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class AllocationEntity
    {
        public Allocation ToModel() =>
            new Allocation(
                Symbol.ToModel(),
                Proportions.Select(p => p.ToModel()));
    }
}
