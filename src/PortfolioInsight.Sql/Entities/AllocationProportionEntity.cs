using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class AllocationProportionEntity : Entity<AllocationProportionEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

        public AllocationProportion ToModel() =>
            new AllocationProportion(
                AssetClass.ToModel(),
                (Rate)Rate);

        public AllocationProportionEntity Assign(AllocationProportion proportion)
        {
            AssetClassId = proportion.AssetClass.Id;
            Rate = proportion.Rate;
            return this;
        }
    }
}
