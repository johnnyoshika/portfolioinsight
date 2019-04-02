using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class Portfolio
    {
        public Portfolio(int authorizationId, IEnumerable<Account> accounts)
        {
            AuthorizationId = authorizationId;
            Accounts = accounts?.ToArray() ?? throw new ArgumentNullException(nameof(accounts));
        }

        public int AuthorizationId { get; }
        public IReadOnlyList<Account> Accounts { get; }
    }
}
