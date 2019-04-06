using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Brokerages;

namespace PortfolioInsight
{
    public partial class BrokerageEntity : Entity<BrokerageEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

        public Brokerage ToDto() =>
            new Brokerage
            {
                Id = Id,
                Name = Name
            };
    }
}
