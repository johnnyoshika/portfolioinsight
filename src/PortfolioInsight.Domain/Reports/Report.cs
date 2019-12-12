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
        public Report(IReadOnlyList<AccountReport> accounts, IReadOnlyList<Allocation> allocations, AssetClass cash, IReadOnlyList<Currency> currencies,  Currency output)
        {
            Accounts = accounts;
            Allocations = allocations;
            Cash = cash;
            Currencies = currencies;
            Output = output;
        }

        public IReadOnlyList<AccountReport> Accounts { get; }
        public IReadOnlyList<Allocation> Allocations { get; }
        public AssetClass Cash { get; }
        public IReadOnlyList<Currency> Currencies { get; }
        public Currency Output { get; }

        public IReadOnlyList<PositionReport> Positions =>
            Accounts
                .Where(a => !a.Exclude)
                .SelectMany(a => a.Positions)
                .Where(r => !r.Exclude)
                .OrderByDescending(r => r.Position.ValueIn(Output)).ToList();

        public IReadOnlyList<BalanceReport> Balances =>
            Accounts.Where(a => !a.Exclude)
                .SelectMany(a => a.Balances)
                .Where(r => !r.Exclude)
                .OrderBy(r => r.Balance.Currency.Code)
                .ToList();

        public Amount PositionTotal =>
            Positions.Sum(r => r.Position.ValueIn(Output));

        public Amount BalanceTotal =>
            Balances.Sum(r => r.Balance.ValueIn(Output));

        public Amount Total =>
            PositionTotal + BalanceTotal;

        public IReadOnlyList<Asset> PositionAssets =>
            Positions
                .GroupBy(r => r.Position.Symbol)
                .Select(g => new { Symbol = g.Key, Value = g.Sum(r => r.Position.ValueIn(Output)) })
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
                .Where(r => r.Balance.Type == Balance.Cash)
                .Select(r => new { Value = r.Balance.ValueIn(Output) })
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
