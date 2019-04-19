using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Financial
{
    public interface ICurrencyReader
    {
        Task<Currency> ReadByCodeAsync(string code);
        Task<List<Currency>> ReadAllAsync();
    }
}
