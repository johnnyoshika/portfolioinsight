using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Reports
{
    public interface IReportWriter
    {
        Task WriteAsync(int portfolioId, DateTime date, Report report);
    }
}
