using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Reports
{
    [Service]
    public class ReportReader : IReportReader
    {

        public ReportReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<Report> ReadSnapshotAsync(int portfolioId, DateTime date) =>
            await ReadByAsync(r => r.PortfolioId == portfolioId && r.Date == date);

        public async Task<List<Report>> ReadByPortfolioIdAsync(int portfolioId, DateTime since) =>
            await ReadManyByAsync(r => r.PortfolioId == portfolioId && r.Date >= since);

        async Task<Report> ReadByAsync(Expression<Func<ReportEntity, bool>> filter)
        {
            using (var context = Context())
                return await context
                    .Reports
                    .Where(filter)
                    .Select(e => e.ToModel())
                    .FirstOrDefaultAsync();
        }

        async Task<List<Report>> ReadManyByAsync(Expression<Func<ReportEntity, bool>> filter)
        {
            using (var context = Context())
                return await context
                    .Reports
                    .Where(filter)
                    .Select(c => c.ToModel())
                    .ToListAsync();
        }
    }
}
