using PortfolioInsight.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioInsight.Reports
{
    public class AccountReport
    {
        public AccountReport(bool exclude, int id, string number, string name, IEnumerable<BalanceReport> balances, IEnumerable<PositionReport> positions)
        {
            Exclude = exclude;
            Id = id;
            Number = number;
            Name = name;
            Balances = balances;
            Positions = positions;
        }

        public bool Exclude { get; }
        public int Id { get; }
        public string Number { get; }
        public string Name { get; }
        public IEnumerable<BalanceReport> Balances { get; }
        public IEnumerable<PositionReport> Positions { get; }
        public Amount TotalIn(Currency currency) =>
            Positions.Where(p => !p.Exclude).Sum(r => r.Position.ValueIn(currency))
            +
            Balances.Where(b => !b.Exclude).Sum(r => r.Balance.ValueIn(currency));
    }
}
