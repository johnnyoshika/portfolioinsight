using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Connections
{
    public interface IConnectionReader
    {
        Task<List<Connection>> ReadAllAsync();
        Task<Connection> ReadByIdAsync(int id);
        Task<Connection> ReadByUserBrokerageAsync(int userId, int brokerageId, string brokerargeUserId);
        Task<List<Connection>> ReadByUserAsync(int userId);
    }
}
