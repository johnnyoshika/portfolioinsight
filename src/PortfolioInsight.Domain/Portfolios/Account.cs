using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class Account
    {
        public Account(int id, string number, string name, IEnumerable<Balance> balances, IEnumerable<Position> positions)
        {
            Id = id;
            Number = number ?? throw new ArgumentNullException(nameof(name));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balances = balances?.ToArray() ?? throw new ArgumentNullException(nameof(balances));
            Positions = positions?.ToArray() ?? throw new ArgumentNullException(nameof(positions)); ;
        }

        public int Id { get; }
        public string Number { get; }
        public string Name { get; }
        public IReadOnlyList<Balance> Balances { get; }
        public IReadOnlyList<Position> Positions { get; }
    }
}
