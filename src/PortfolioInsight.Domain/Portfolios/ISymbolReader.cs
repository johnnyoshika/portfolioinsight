using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface ISymbolReader
    {
        Task<Symbol> ReadByIdAsync(int id);
        Task<Symbol> ReadByBrokerageReferenceAsync(int brokerageId, string referenceId);
        Task<Symbol> ReadByBrokerageNameAsync(int brokerageId, string name);
    }
}
