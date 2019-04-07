using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface ISymbolReader
    {
        Task<Symbol> ReadByIdAsync(int id);
        Task<Symbol> ReadByNameAtListingExchangeAsync(string name, string listingExchangeCode);
        Task<Symbol> ReadByBrokerageReferenceAsync(int brokerageId, string referenceId);
    }
}
