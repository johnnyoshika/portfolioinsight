using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Brokerages
{
    public class Brokerage
    {
        public static readonly Brokerage Questrade = new Brokerage { Id = 1, Name = "Questrade" };

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
