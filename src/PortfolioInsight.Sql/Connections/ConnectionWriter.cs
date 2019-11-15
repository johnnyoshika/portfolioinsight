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
                var eConnection = await context
                    .Connections
                    .Where(a => a.Id == connection.Id)
                    .FirstOrDefaultAsync();

                if (eConnection == null)
                {
                    eConnection = new ConnectionEntity();
                    context.Connections.Add(eConnection);
                }

                eConnection.Assign(connection);
                await context.SaveChangesAsync();
                connection.Id = eConnection.Id;
            }
        }
    }
}
