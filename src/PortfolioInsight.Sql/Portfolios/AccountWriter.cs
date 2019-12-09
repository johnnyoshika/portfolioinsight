using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class AccountWriter : IAccountWriter
    {
        public AccountWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(int connectionId, List<Account> accounts)
        {
            using (var context = Context())
            {
                var eConnection = await context
                    .Connections
                    .Include(c => c.Accounts)
                        .ThenInclude(a => a.Balances)
                    .Include(c => c.Accounts)
                        .ThenInclude(a => a.Positions)
                    .Where(c => c.Id == connectionId)
                    .FirstAsync();

                context.Balances.RemoveRange(eConnection.Accounts.SelectMany(a => a.Balances));
                context.Positions.RemoveRange(eConnection.Accounts.SelectMany(a => a.Positions));
                context.Accounts.RemoveRange(
                    eConnection.Accounts.Where(e => !accounts.Any(a => a.Id == e.Id)));

                foreach (var a in accounts)
                {
                    var eAccount = a.Id == 0 ? null : eConnection.Accounts.FirstOrDefault(e => e.Id == a.Id);
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
