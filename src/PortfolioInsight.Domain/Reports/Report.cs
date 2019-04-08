using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight.Reports
{
    public class Report
    {
        public Report(IReadOnlyList<Portfolio> portfolios, IReadOnlyList<Allocation> allocations, Currency currency)
        {
            Portfolios = portfolios;
            Allocations = allocations;
            Currency = currency;
        }

        public IReadOnlyList<Portfolio> Portfolios { get; }
        public IReadOnlyList<Allocation> Allocations { get; }
        public Currency Currency { get; }

        public IReadOnlyList<Account> Accounts =>
            Portfolios.SelectMany(p => p.Accounts).ToList();

        public IReadOnlyList<Position> Positions =>
            Accounts.SelectMany(a => a.Positions).ToList();

        public IReadOnlyList<Balance> Balances =>
            Accounts.SelectMany(a => a.Balances).ToList();

        public Amount PositionTotal =>
            Positions.Sum(p => p.ValueIn(Currency));

        public Amount BalanceTotal =>
            Balances.Sum(b => b.ValueIn(Currency));

        public Amount Total =>
            PositionTotal + BalanceTotal;

        public IReadOnlyList<Asset> PositionAssets =>
            Positions
                .GroupBy(p => p.Symbol)
                .Select(g => new { Symbol = g.Key, Value = g.Sum(p => p.ValueIn(Currency)) })
                .SelectMany(sv =>
                    (
                        Allocations.FirstOrDefault(a => a.Symbol == sv.Symbol)?.Proportions
                        ??
                        new List<AllocationProportion>
                        {
                            new AllocationProportion(AssetClass.Unknown, Rate.Full)
                        }
                    )
                    .Select(proportion => new { proportion.AssetClass, Value = sv.Value * proportion.Rate }))
                .GroupBy(av => av.AssetClass)
                .Select(g => new { AssetClass = g.Key, Value = g.Sum(av => av.Value) })
                .Select(av => new Asset(av.AssetClass, av.Value, (Rate)(av.Value / PositionTotal.Value)))
                .ToList();

        public IReadOnlyList<Asset> BalanceAssets => throw new NotImplementedException();

        public IReadOnlyList<Asset> Assets =>
            PositionAssets
                .Concat(BalanceAssets)
                .Select(a => new Asset(a.AssetClass, a.Value, (Rate)(a.Value.Value / Total.Value)))
                .ToList();
    }
}
