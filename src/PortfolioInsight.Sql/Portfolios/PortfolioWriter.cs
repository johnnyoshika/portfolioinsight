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
                    .Where(a => a.Id == portfolio.AuthorizationId)
                    .FirstOrDefaultAsync();

                context.Accounts.RemoveRange(eAuthorization.Accounts);
                eAuthorization.Accounts.AddRange(
                    portfolio.Accounts.Select(a => new AccountEntity().Assign(a)));

                await context.SaveChangesAsync();
            }
        }
    }
}
