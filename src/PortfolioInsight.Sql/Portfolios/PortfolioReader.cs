using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class PortfolioReader : IPortfolioReader
    {
        public PortfolioReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<Portfolio> ReadByConnectionIdAsync(int connectionId)
        {
            using (var context = Context())
                return new Portfolio(connectionId, await context
                    .Accounts
                    .Include(a => a.Balances)
                        .ThenInclude(b => b.Currency)
                    .Include(a => a.Positions)
                        .ThenInclude(p => p.Symbol.Currency)
                    .Where(a => a.AuthorizationId == connectionId)
                    .Select(a => a.ToModel())
                    .ToListAsync());
        }
    }
}
