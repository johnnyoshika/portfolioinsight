using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Connections
{
    public interface IConnectionWriter
    {
        Task WriteAsync(Connection connection);
    }
}
