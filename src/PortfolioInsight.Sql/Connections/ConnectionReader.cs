using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Connections
{
    [Service]
    public class ConnectionReader : IConnectionReader
    {
        public ConnectionReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<List<Connection>> ReadAllAsync()
        {
            using (var context = Context())
                return await IncludeGraph(context)
                    .Select(a => a.ToDto())
                    .ToListAsync();
        }

        public async Task<Connection> ReadByIdAsync(int id)
        {
            using (var context = Context())
                return await IncludeGraph(context)
                    .Where(a => a.Id == id)
                    .Select(a => a.ToDto())
                    .FirstOrDefaultAsync();
        }

        public async Task<Connection> ReadByUserBrokerageAsync(int userId, int brokerageId, string brokerargeUserId)
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

        public async Task<List<Connection>> ReadByUserIdAsync(int userId)
        {
            using (var context = Context())
                return await IncludeGraph(context)
                    .Where(a => a.UserId == userId)
                    .Select(a => a.ToDto())
                    .ToListAsync();
        }

        IQueryable<ConnectionEntity> IncludeGraph(Context context) =>
            context
                .Connections
                .Include(a => a.User)
                .Include(a => a.Brokerage);
    }
}
