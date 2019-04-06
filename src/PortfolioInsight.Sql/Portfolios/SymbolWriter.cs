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

        public async Task<Symbol> WriteAsync(string name, string listingExchangeCode, string currencyCode, int brokerageId, string referenceId)
        {
            using (var context = Context())
            {
                var eSymbol = await context
                    .Symbols
                    .Where(s => s.Name == name && s.ListingExchangeCode == listingExchangeCode)
                    .FirstOrDefaultAsync();

                if (eSymbol == null)
                {
                    eSymbol = new SymbolEntity
                    {
                        Name = name,
                        ListingExchangeCode = listingExchangeCode,
                        CurrencyCode = currencyCode,
                        BrokerageSymbols = new List<BrokerageSymbolEntity>()
                    };
                    context.Symbols.Add(eSymbol);
                }

                if (!eSymbol.BrokerageSymbols.Any(bs => bs.BrokerageId == brokerageId))
                    eSymbol.BrokerageSymbols.Add(new BrokerageSymbolEntity
                    {
                        BrokerageId = brokerageId,
                        ReferenceId = referenceId
                    });

                await context.SaveChangesAsync();
                return eSymbol.ToDto();
            }
        }
    }
}
