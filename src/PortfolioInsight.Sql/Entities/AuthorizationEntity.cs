﻿using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Connections;

namespace PortfolioInsight
{
    public partial class AuthorizationEntity
    {
        public Connection ToDto() =>
            new Connection
            {
                Id = Id,
                User = User.ToDto(),
                Brokerage = Brokerage.ToDto(),
                BrokerageUserId = BrokerageUserId,
                RefreshToken = RefreshToken
            };

        public AuthorizationEntity Assign(Connection connection)
        {
            UserId = connection.User.Id;
            BrokerageId = connection.Brokerage.Id;
            BrokerageUserId = connection.BrokerageUserId;
            RefreshToken = connection.RefreshToken;
            return this;
        }
    }
}
