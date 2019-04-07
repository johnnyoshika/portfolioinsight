using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IAllocationWriter
    {
        Task WriteAsync(int userId, Allocation allocation);
    }
}
