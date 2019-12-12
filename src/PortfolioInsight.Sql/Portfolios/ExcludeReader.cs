using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class ExcludeReader : IExcludeReader
    {
        public ExcludeReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<Exclude> ReadByPortfolioIdAsync(int portfolioId)
        {
            using (var context = Context())
                return new Exclude(
                    await context
                        .ExcludeAccounts
                        .Where(a => a.PortfolioId == portfolioId)
                        .Select(a => new ExcludeAccount(a.AccountId))
                        .ToListAsync(),
                    await context
                        .ExcludeSymbols
                        .Where(s => s.PortfolioId == portfolioId)
                        .Select(s => new ExcludeSymbol(s.AccountId, s.SymbolId))
                        .ToListAsync());
        }
    }
}
