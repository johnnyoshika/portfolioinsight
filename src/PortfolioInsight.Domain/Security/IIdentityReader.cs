using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Users;

namespace PortfolioInsight.Security
{
    public interface IIdentityReader
    {
        Task<User> AuthenticateAsync(string email, string password);
    }
}
