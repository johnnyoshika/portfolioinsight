using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Portfolio> ReadByIdAsync(int id)
        {
            using (var context = Context())
                return await context
                    .Portfolios
                    .Where(p => p.Id == id)
                    .Select(p => p.ToModel())
                    .FirstOrDefaultAsync();
        }

        public async Task<List<Portfolio>> ReadByUserIdAsync(int userId)
        {
            using (var context = Context())
                return await context
                    .Portfolios
                    .Where(p => p.UserId == userId)
                    .Select(p => p.ToModel())
                    .ToListAsync();
        }

        public async Task<bool> UserOwnsPortfolio(int portfolioId, int userId)
        {
            using (var context = Context())
                return await context
                    .Portfolios
                    .Where(p => p.Id == portfolioId && p.UserId == userId)
                    .AnyAsync();
        }
    }
}
