using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Financial
{
    [Service]
    public class CurrencyReader : ICurrencyReader
    {
        public CurrencyReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<Currency> ReadByCodeAsync(string code)
        {
            using (var context = Context())
                return await context
                    .Currencies
                    .Where(c => c.Code == code)
                    .Select(c => c.ToModel())
                    .FirstAsync();
        }

        public async Task<List<Currency>> ReadAllAsync()
        {
            using (var context = Context())
                return await context
                    .Currencies
                    .Select(c => c.ToModel())
                    .ToListAsync();

        }
    }
}
