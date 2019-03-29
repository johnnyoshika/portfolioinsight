using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioInsight.Users;

namespace PortfolioInsight.Security
{
    [Service]
    public class IdentityReader : IIdentityReader
    {
        public IdentityReader(Func<Context> context, IPasswordSecurity passwordSecurity)
        {
            Context = context;
            PasswordSecurity = passwordSecurity;
        }

        Func<Context> Context { get; }
        IPasswordSecurity PasswordSecurity { get; }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            using (var context = Context())
            {
                var eUser = await context
                    .Users
                    .Where(u => u.Email == email)
                    .FirstOrDefaultAsync();

                if (eUser == null)
                    throw new AuthenticationException();

                PasswordSecurity.Verify(eUser.PasswordHash, password);

                var now = DateTime.UtcNow;
                eUser.LastActivityAt = now;
                eUser.ActivityCount++;
                eUser.LastLoginAt = now;
                eUser.LoginCount++;
                await context.SaveChangesAsync();

                return eUser.ToDto();
            }
        }
    }
}
