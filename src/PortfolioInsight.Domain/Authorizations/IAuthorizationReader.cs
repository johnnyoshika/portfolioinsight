using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Authorizations
{
    public interface IAuthorizationReader
    {
        Task<List<Authorization>> ReadAllAsync();
        Task<Authorization> ReadByIdAsync(int id);
        Task<Authorization> ReadByUserBrokerageAsync(int userId, int brokerageId, string brokerargeUserId);
        Task<List<Authorization>> ReadByUserAsync(int userId);
    }
}
