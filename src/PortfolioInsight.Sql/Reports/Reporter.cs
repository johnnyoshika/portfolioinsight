using PortfolioInsight.Connections;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Reports
{
    [Service]
    public class Reporter : IReporter
    {
        public Reporter(
            IConnectionReader connectionReader,
            IAccountReader accountReader,
            IExcludeReader excludeReader,
            IAllocationReader allocationReader,
            IAssetClassReader assetClassReader,
            ICurrencyReader currencyReader)
        {
            ConnectionReader = connectionReader;
            AccountReader = accountReader;
            ExcludeReader = excludeReader;
            AllocationReader = allocationReader;
            AssetClassReader = assetClassReader;
            CurrencyReader = currencyReader;
        }

        IConnectionReader ConnectionReader { get; }
        IAccountReader AccountReader { get; }
        IExcludeReader ExcludeReader { get; }
        IAllocationReader AllocationReader { get; }
        IAssetClassReader AssetClassReader { get; }
        ICurrencyReader CurrencyReader { get; }

        public async Task<Report> GenerateAsync(int userId, int portfolioId)
        {
            var connections = ConnectionReader.ReadByUserIdAsync(userId);
            var allocations = AllocationReader.ReadByPortfolioIdAsync(portfolioId);
            var cashAssetClass = AssetClassReader.ReadCashByPortfolioIdAsync(portfolioId);
            var equityAssetClasses = AssetClassReader.ReadEquityByPortfolioIdAsync(portfolioId);
            var currencies = CurrencyReader.ReadAllAsync();
            var exclude = await ExcludeReader.ReadByPortfolioIdAsync(portfolioId);

            var accounts = new List<AccountReport>();
            foreach (var connection in await connections)
                accounts.AddRange(
                    (await AccountReader.ReadByConnectionIdAsync(connection.Id))
                        .Select(a => new AccountReport(
                            exclude.Accounts.Any(e => e.Id == a.Id),
                            a.Id,
                            a.Number,
                            a.Name,
                            a.Balances.Select(b => new BalanceReport(false, b)), // TODO: In the future, get exclude value from database
                            a.Positions.Select(p => new PositionReport(
                                exclude.Symbols.Any(s => s.AccountId == a.Id && s.SymbolId == p.Symbol.Id), p)))));

            return new Report(
                accounts.OrderBy(a => a.Name).ToList(),
                await allocations,
                await cashAssetClass,
                await equityAssetClasses,
                await currencies,
                (await currencies).First(c => c.Code == "CAD"));
        }
    }
}
