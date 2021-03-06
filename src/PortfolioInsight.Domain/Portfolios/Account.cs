﻿using PortfolioInsight.Financial;
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
            Balances = balances?.OrderBy(b => b.Currency.Code).ToArray() ?? throw new ArgumentNullException(nameof(balances));
            Positions = positions?.OrderBy(p => p.Symbol.Name).ToArray() ?? throw new ArgumentNullException(nameof(positions)); ;
        }

        public int Id { get; }
        public string Number { get; }
        public string Name { get; }
        public IReadOnlyList<Balance> Balances { get; }
        public IReadOnlyList<Position> Positions { get; }
        public Amount TotalIn(Currency currency) =>
            Positions.Sum(p => p.ValueIn(currency))
            +
            Balances.Sum(b => b.ValueIn(currency));
    }
}
