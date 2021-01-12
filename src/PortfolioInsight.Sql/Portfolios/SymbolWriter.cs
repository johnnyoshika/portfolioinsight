using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class SymbolWriter : ISymbolWriter
    {
        public SymbolWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<Symbol> WriteAsync(NewSymbol newSymbol)
        {
            using (var context = Context())
            {
                var eSymbol = await context
                    .Symbols
                    .Include(s => s.Currency)
                    .Where(s => s.Name == newSymbol.Name && s.ListingExchangeCode == newSymbol.ListingExchangeCode)
                    .FirstOrDefaultAsync();

                if (eSymbol == null)
                {
                    eSymbol = new SymbolEntity
                    {
                        Name = newSymbol.Name,
                        Description = newSymbol.Description,
                        ListingExchangeCode = newSymbol.ListingExchangeCode,
                        CurrencyCode = newSymbol.CurrencyCode,
                        Currency = await context.Currencies.FirstAsync(c => c.Code == newSymbol.CurrencyCode),
                        BrokerageSymbols = new List<BrokerageSymbolEntity>()
                    };
                    context.Symbols.Add(eSymbol);
                }

                if (!eSymbol.BrokerageSymbols.Any(bs => bs.BrokerageId == newSymbol.BrokerageId))
                    eSymbol.BrokerageSymbols.Add(new BrokerageSymbolEntity
                    {
                        BrokerageId = newSymbol.BrokerageId,
                        ReferenceId = newSymbol.ReferenceId
                    });

                await context.SaveChangesAsync();
                return eSymbol.ToModel();
            }
        }
    }
}
