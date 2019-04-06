using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Connections;

namespace PortfolioInsight
{
    public partial class ConnectionEntity : Entity<ConnectionEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

        public Connection ToDto() =>
            new Connection
            {
                Id = Id,
                User = User.ToDto(),
                Brokerage = Brokerage.ToDto(),
                BrokerageUserId = BrokerageUserId,
                RefreshToken = RefreshToken
            };

        public ConnectionEntity Assign(Connection connection)
        {
            UserId = connection.User.Id;
            BrokerageId = connection.Brokerage.Id;
            BrokerageUserId = connection.BrokerageUserId;
            RefreshToken = connection.RefreshToken;
            return this;
        }
    }
}
