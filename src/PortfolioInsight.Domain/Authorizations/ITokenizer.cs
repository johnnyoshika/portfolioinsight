using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Users;

namespace PortfolioInsight.Authorizations
{
    public interface ITokenizer
    {
        Task<AccessToken> ExchangeAsync(string code, User user, string redirectUrl);
        Task<AccessToken> RefreshAsync(Authorization authorization);
    }
}
