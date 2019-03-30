using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Authorizations;

namespace PortfolioInsight
{
    public partial class AuthorizationEntity
    {
        public Authorization ToDto() =>
            new Authorization
            {
                Id = Id,
                User = User.ToDto(),
                Brokerage = Brokerage.ToDto(),
                BrokerageUserId = BrokerageUserId,
                RefreshToken = RefreshToken,
                IsValid = IsValid
            };

        public AuthorizationEntity Assign(Authorization authorization)
        {
            UserId = authorization.User.Id;
            BrokerageId = authorization.Brokerage.Id;
            BrokerageUserId = authorization.BrokerageUserId;
            RefreshToken = authorization.RefreshToken;
            IsValid = authorization.IsValid;
            return this;
        }
    }
}
