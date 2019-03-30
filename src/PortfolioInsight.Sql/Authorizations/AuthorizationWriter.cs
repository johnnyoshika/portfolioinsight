using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Authorizations
{
    [Service]
    public class AuthorizationWriter : IAuthorizationWriter
    {
        public AuthorizationWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(Authorization authorization)
        {
            using (var context = Context())
            {
                var eAuthorization = await context
                    .Authorizations
                    .Where(a => a.Id == authorization.Id)
                    .FirstOrDefaultAsync();

                if (eAuthorization == null)
                {
                    eAuthorization = new AuthorizationEntity();
                    context.Authorizations.Add(eAuthorization);
                }

                eAuthorization.Assign(authorization);
                await context.SaveChangesAsync();
                authorization.Id = eAuthorization.Id;
            }
        }
    }
}
