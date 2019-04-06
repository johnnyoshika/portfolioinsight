using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class AccountEntity : Entity<AccountEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

        public Account ToModel() =>
            new Account(
                Id,
                Number,
                Name,
                Balances.Select(b => b.ToModel()),
                Positions.Select(p => p.ToModel()));

        public AccountEntity Assign(Account account)
        {
            Number = account.Number;
            Name = account.Name;
            Balances = account.Balances.Select(b => new BalanceEntity().Assign(b)).ToList();
            Positions = account.Positions.Select(p => new PositionEntity().Assign(p)).ToList();
            return this;
        }
    }
}
