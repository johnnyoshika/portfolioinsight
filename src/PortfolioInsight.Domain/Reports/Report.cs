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
        public Report(IReadOnlyList<Portfolio> portfolios, IReadOnlyList<Allocation> allocations, AssetClass cash, IReadOnlyList<Currency> currencies,  Currency output)
        {
            Portfolios = portfolios;
            Allocations = allocations;
            Cash = cash;
            Currencies = currencies;
            Output = output;
        }

        public IReadOnlyList<Portfolio> Portfolios { get; }
        public IReadOnlyList<Allocation> Allocations { get; }
        public AssetClass Cash { get; }
        public IReadOnlyList<Currency> Currencies { get; }
        public Currency Output { get; }

        public IReadOnlyList<Account> Accounts =>
            Portfolios.SelectMany(p => p.Accounts).ToList();

        public IReadOnlyList<Position> Positions =>
            Accounts.SelectMany(a => a.Positions).ToList();

        public IReadOnlyList<Balance> Balances =>
            Accounts.SelectMany(a => a.Balances).ToList();

        public Amount PositionTotal =>
            Positions.Sum(p => p.ValueIn(Output));

        public Amount BalanceTotal =>
            Balances.Sum(b => b.ValueIn(Output));

        public Amount Total =>
            PositionTotal + BalanceTotal;

        public IReadOnlyList<Asset> PositionAssets =>
            Positions
                .GroupBy(p => p.Symbol)
                .Select(g => new { Symbol = g.Key, Value = g.Sum(p => p.ValueIn(Output)) })
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
                .OrderByDescending(a => a.Value)
                .ToList();

        public IReadOnlyList<Asset> BalanceAssets =>
            Balances
                .Where(b => b.Type == Balance.Cash)
                .Select(b => new { Value = b.ValueIn(Output) })
                .GroupBy(b => true)
                .Select(g => new { AssetClass = Cash, Value = g.Sum(v => v.Value) })
                .Select(av => new Asset(av.AssetClass, av.Value, (Rate)(av.Value / BalanceTotal.Value)))
                .OrderByDescending(a => a.Value)
                .ToList();

        public IReadOnlyList<Asset> Assets =>
            PositionAssets
                .Concat(BalanceAssets)
                .Select(a => new Asset(a.AssetClass, a.Value, (Rate)(a.Value.Value / Total.Value)))
                .ToList();
    }
}
