using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Reports
{
    public interface IReporter
    {
        Task<Report> GenerateAsync(int userId, int portfolioId);
    }
}
