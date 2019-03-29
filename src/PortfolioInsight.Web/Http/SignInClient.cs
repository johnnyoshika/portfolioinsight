using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using PortfolioInsight.Users;

namespace PortfolioInsight.Web.Http
{
    [Service]
    public class SignInClient : ISignInClient
    {
        public async Task SignInAsync(HttpContext context, User user, bool isPersistent)
        {
            // https://andrewlock.net/introduction-to-authentication-with-asp-net-core/
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("UserId", user.Id.ToString(), ClaimValueTypes.Integer)
            }, "Cookie");

            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(principal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMonths(6),
                IsPersistent = isPersistent
            });
        }
    }
}
