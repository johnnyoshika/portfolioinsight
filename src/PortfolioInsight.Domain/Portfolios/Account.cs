using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class Account
    {
        public Account(string number, string name, IEnumerable<Balance> balances, IEnumerable<Position> positions)
        {
            Number = number ?? throw new ArgumentNullException(nameof(name));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balances = balances?.ToArray() ?? throw new ArgumentNullException(nameof(balances));
            Positions = positions?.ToArray() ?? throw new ArgumentNullException(nameof(positions)); ;
        }

        public string Number { get; }
        public string Name { get; }
        public IReadOnlyList<Balance> Balances { get; }
        public IReadOnlyList<Position> Positions { get; }
    }
}
