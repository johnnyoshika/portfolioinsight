using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Reports
{
    public interface IReportReader
    {
        Task<Report> ReadSnapshotAsync(int portfolioId, DateTime date);
        Task<List<Report>> ReadByPortfolioIdAsync(int portfolioId, DateTime since);
    }
}
