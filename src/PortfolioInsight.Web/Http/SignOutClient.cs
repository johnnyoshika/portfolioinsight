using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace PortfolioInsight.Web.Http
{
    [Service]
    public class SignOutClient : ISignOutClient
    {
        public async Task SignOutAsync(HttpContext context) => 
            await context.SignOutAsync();
    }
}
