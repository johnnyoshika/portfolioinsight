using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortfolioInsight.Users;

namespace PortfolioInsight.Web.Http
{
    public interface IAuthenticationClient
    {
        Task<User> AuthenticateAsync(HttpRequest request);
    }
}
