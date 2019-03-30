using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Authorizations
{
    public interface IAuthorizationReader
    {
        Task<Authorization> ReadByIdAsync(int id);
        Task<Authorization> ReadByUserBrokerage(int userId, int brokerageId, string brokerargeUserId);
    }
}
