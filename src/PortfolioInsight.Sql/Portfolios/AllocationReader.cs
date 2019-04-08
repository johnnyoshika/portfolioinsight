using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class AllocationReader : IAllocationReader
    {
        public AllocationReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<List<Allocation>> ReadByUserIdAsync(int userId)
        {
            using (var context = Context())
                return await context
                    .Allocations
                    .Include(a => a.Symbol)
                        .ThenInclude(s => s.Currency)
                    .Include(a => a.Proportions)
                        .ThenInclude(p => p.AssetClass)
                    .Where(a => a.UserId == userId)
                    .Select(a => a.ToModel())
                    .ToListAsync();
        }
    }
}
