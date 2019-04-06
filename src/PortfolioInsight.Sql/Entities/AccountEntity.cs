using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class AccountEntity
    {
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
