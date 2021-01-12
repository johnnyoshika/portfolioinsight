using PortfolioInsight.Connections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public interface ISymbolReader
    {
        Task<Symbol> ReadByIdAsync(int id);
        Task<Symbol> ReadByNameAsync(string name);

        /// <summary>
        /// Same as ReadByNameAsync(string name) except a Symbol will be fetched and saved in the database if it doesn't exist.
        /// </summary>
        /// <param name="name">Symbol name</param>
        /// <param name="connection">Connection to use to fetch symbol if it doesn't exist in the database</param>
        /// <returns></returns>
        Task<Symbol> ReadByNameAsync(string name, Connection connection);
        Task<Symbol> ReadByBrokerageReferenceAsync(int brokerageId, string referenceId);
    }
}
