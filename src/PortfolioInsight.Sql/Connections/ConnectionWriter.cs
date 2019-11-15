using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Connections
{
    [Service]
    public class ConnectionWriter : IConnectionWriter
    {
        public ConnectionWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(Connection connection)
        {
            using (var context = Context())
            {
                var eAuthorization = await context
                    .Authorizations
                    .Where(a => a.Id == connection.Id)
                    .FirstOrDefaultAsync();

                if (eAuthorization == null)
                {
                    eAuthorization = new AuthorizationEntity();
                    context.Authorizations.Add(eAuthorization);
                }

                eAuthorization.Assign(connection);
                await context.SaveChangesAsync();
                connection.Id = eAuthorization.Id;
            }
        }
    }
}
