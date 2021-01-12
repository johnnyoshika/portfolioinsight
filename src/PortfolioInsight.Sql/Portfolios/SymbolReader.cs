using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class SymbolReader : ISymbolReader
    {
        public SymbolReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<Symbol> ReadByIdAsync(int id) =>
            await ReadByAsync(s => s.Id == id);

        public async Task<Symbol> ReadByNameAsync(string name) =>
            await ReadByAsync(s => s.Name == name);

        public async Task<Symbol> ReadByBrokerageReferenceAsync(int brokerageId, string referenceId) =>
            await ReadByAsync(s => s.BrokerageSymbols.Any(bs => bs.BrokerageId == brokerageId && bs.ReferenceId == referenceId));

        async Task<Symbol> ReadByAsync(Expression<Func<SymbolEntity, bool>> filter)
        {
            using (var context = Context())
                return await context
                    .Symbols
                    .Include(s => s.Currency)
                    .Where(filter)
                    .Select(e => e.ToModel())
                    .FirstOrDefaultAsync();
        }
    }
}
