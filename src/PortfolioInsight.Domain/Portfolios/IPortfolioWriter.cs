using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface IPortfolioWriter
    {
        Task WriteAsync(Portfolio portfolio);
    }
}
