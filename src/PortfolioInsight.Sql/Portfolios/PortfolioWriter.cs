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
                var eAuthorization = await context
                    .Authorizations
                    .Include(a => a.Accounts)
                        .ThenInclude(a => a.Balances)
                    .Include(a => a.Accounts)
                        .ThenInclude(a => a.Positions)
                    .Where(a => a.Id == portfolio.ConnectionId)
                    .FirstAsync();

                context.Balances.RemoveRange(eAuthorization.Accounts.SelectMany(a => a.Balances));
                context.Positions.RemoveRange(eAuthorization.Accounts.SelectMany(a => a.Positions));
                context.Accounts.RemoveRange(
                    eAuthorization.Accounts.Where(e => !portfolio.Accounts.Any(a => a.Id == e.Id)));

                foreach (var a in portfolio.Accounts)
                {
                    var eAccount = eAuthorization.Accounts.FirstOrDefault(e => e.Id == a.Id);

                    if (eAccount == null)
                    {
                        eAccount = new AccountEntity
                        {
                            AuthorizationId = eAuthorization.Id,
                            Authorization = eAuthorization
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
