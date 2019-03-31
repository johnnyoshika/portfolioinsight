using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Brokerages;
using PortfolioInsight.Users;

namespace PortfolioInsight.Authorizations
{
    public class Authorization
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Brokerage Brokerage { get; set; }
        public string BrokerageUserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
