using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortfolioInsight.Users;

namespace PortfolioInsight.Web.Http
{
    [Service]
    public class AuthenticationClient : IAuthenticationClient
    {
        public AuthenticationClient(IUserReader userReader)
        {
            UserReader = userReader;
        }

        IUserReader UserReader { get; }

        public async Task<User> AuthenticateAsync(HttpRequest request)
        {
            var identity = (ClaimsIdentity)request.HttpContext.User.Identity;
            if (!identity.IsAuthenticated)
                throw new UnauthorizedAccessException();

            string token = identity.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!int.TryParse(token, out int userId))
                throw new UnauthorizedAccessException();

            return await UserReader.ReadByIdAsync(userId) ?? throw new UnauthorizedAccessException();
        }
    }
}
