using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class Portfolio
    {
        public Portfolio(int connectionId, IEnumerable<Account> accounts)
        {
            ConnectionId = connectionId;
            Accounts = accounts?.ToArray() ?? throw new ArgumentNullException(nameof(accounts));
        }

        public int ConnectionId { get; }
        public IReadOnlyList<Account> Accounts { get; }
    }
}
