using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Authorizations
{
    [Service]
    public class AuthorizationReader : IAuthorizationReader
    {
        public AuthorizationReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<List<Authorization>> ReadAllAsync()
        {
            using (var context = Context())
                return await IncludeGraph(context)
                    .Select(a => a.ToDto())
                    .ToListAsync();
        }

        public async Task<Authorization> ReadByIdAsync(int id)
        {
            using (var context = Context())
                return await IncludeGraph(context)
                    .Where(a => a.Id == id)
                    .Select(a => a.ToDto())
                    .FirstOrDefaultAsync();
        }

        public async Task<Authorization> ReadByUserBrokerageAsync(int userId, int brokerageId, string brokerargeUserId)
        {
            using (var context = Context())
                return await IncludeGraph(context)
                    .Where(a =>
                        a.UserId == userId
                        &&
                        a.BrokerageId == brokerageId
                        &&
                        a.BrokerageUserId == brokerargeUserId)
                    .Select(a => a.ToDto())
                    .FirstOrDefaultAsync();
        }

        IQueryable<AuthorizationEntity> IncludeGraph(Context context) =>
            context
                .Authorizations
                .Include(a => a.User)
                .Include(a => a.Brokerage);
    }
}
