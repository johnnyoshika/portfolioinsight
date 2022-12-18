using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Reports
{
    [Service]
    public class ReportWriter : IReportWriter
    {
        public ReportWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(int portfolioId, DateTime date, Report report)
        {
            using (var context = Context())
            {
                var eReport = await context
                    .Reports
                    .Where(r => r.PortfolioId == portfolioId && r.Date == date)
                    .FirstOrDefaultAsync();

                if (eReport == null)
                {
                    eReport = new ReportEntity
                    {
                        PortfolioId = portfolioId,
                        Date = date,
                    };
                    context.Reports.Add(eReport);
                }

                eReport.Assign(DateTime.UtcNow, report);
                await context.SaveChangesAsync();
            }
        }
    }
}
