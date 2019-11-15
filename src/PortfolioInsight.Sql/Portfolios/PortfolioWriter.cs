using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class PortfolioWriter : IPortfolioWriter
    {
        public PortfolioWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(Portfolio portfolio)
        {
            using (var context = Context())
            {
                var eConnection = await context
                    .Connections
                    .Include(a => a.Accounts)
                        .ThenInclude(a => a.Balances)
                    .Include(a => a.Accounts)
                        .ThenInclude(a => a.Positions)
                    .Where(a => a.Id == portfolio.ConnectionId)
                    .FirstAsync();

                context.Balances.RemoveRange(eConnection.Accounts.SelectMany(a => a.Balances));
                context.Positions.RemoveRange(eConnection.Accounts.SelectMany(a => a.Positions));
                context.Accounts.RemoveRange(
                    eConnection.Accounts.Where(e => !portfolio.Accounts.Any(a => a.Id == e.Id)));

                foreach (var a in portfolio.Accounts)
                {
                    var eAccount = eConnection.Accounts.FirstOrDefault(e => e.Id == a.Id);

                    if (eAccount == null)
                    {
                        eAccount = new AccountEntity
                        {
                            ConnectionId = eConnection.Id,
                            Connection = eConnection
                        };
                        context.Accounts.Add(eAccount);
                    }

                    eAccount.Assign(a);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
