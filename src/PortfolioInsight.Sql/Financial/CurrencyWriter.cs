using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Financial
{
    [Service]
    public class CurrencyWriter : ICurrencyWriter
    {
        public CurrencyWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(Currency currency)
        {
            using (var context = Context())
            {
                var eCurrency = await context
                    .Currencies
                    .Where(c => c.Code == currency.Code)
                    .FirstAsync();

                eCurrency.Rate = currency.Rate;
                eCurrency.AsOf = currency.AsOf;
                await context.SaveChangesAsync();
            }
        }
    }
}
