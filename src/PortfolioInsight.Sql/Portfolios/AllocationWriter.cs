using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class AllocationWriter : IAllocationWriter
    {
        public AllocationWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(int userId, Allocation allocation)
        {
            using (var context = Context())
            {
                var eAllocation = await context
                    .Allocations
                    .Include(a => a.Proportions)
                    .Where(a => a.UserId == userId && a.SymbolId == allocation.Symbol.Id)
                    .FirstOrDefaultAsync();

                if (eAllocation == null)
                {
                    eAllocation = new AllocationEntity
                    {
                        UserId = userId,
                        SymbolId = allocation.Symbol.Id,
                        Proportions = new List<AllocationProportionEntity>()
                    };
                    context.Allocations.Add(eAllocation);
                }

                context.AllocationProportions.RemoveRange(eAllocation.Proportions);
                eAllocation.Proportions = allocation.Proportions.Select(p => new AllocationProportionEntity().Assign(p)).ToList();
                await context.SaveChangesAsync();
            }
        }
    }
}
