using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Brokerages;

namespace PortfolioInsight
{
    public partial class BrokerageEntity
    {
        public Brokerage ToDto() =>
            new Brokerage
            {
                Id = Id,
                Name = Name
            };
    }
}
