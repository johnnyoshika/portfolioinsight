using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Connections;
using PortfolioInsight.Brokerages;
using PortfolioInsight.Financial;
using PortfolioInsight.Http;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class PortfolioSynchronizer : IPortfolioSynchronizer
    {
        public PortfolioSynchronizer(
            IPortfolioReader portfolioReader,
            IPortfolioWriter portfolioWriter,
            ISymbolReader symbolReader,
            ISymbolWriter symbolWriter,
            ICurrencyReader currencyReader,
            ITokenizer tokenizer)
        {
            PortfolioReader = portfolioReader;
            PortfolioWriter = portfolioWriter;
            SymbolReader = symbolReader;
            SymbolWriter = symbolWriter;
            CurrencyReader = currencyReader;
            Tokenizer = tokenizer;
        }

        IPortfolioReader PortfolioReader { get; }
        IPortfolioWriter PortfolioWriter { get; }
        ISymbolReader SymbolReader { get; }
        ISymbolWriter SymbolWriter { get; }
        ICurrencyReader CurrencyReader { get; }
        ITokenizer Tokenizer { get; }

        public async Task SyncAsync(Connection connection)
        {
            var portfolio = await PortfolioReader.ReadByConnectionIdAsync(connection.Id);
            await PortfolioWriter.WriteAsync(
                new Portfolio(
                    connection.Id,
                    await SyncAccounts(portfolio.Accounts, await Tokenizer.RefreshAsync(connection))));
        }


        async Task<IEnumerable<Account>> SyncAccounts(IEnumerable<Account> accounts, AccessToken accessToken)
        {
            var fresh = new List<Account>();
            foreach (var a in (await AccountApi.FindAccountsAsync(accessToken)).Accounts)
            {
                var account = accounts.FirstOrDefault(_ => _.Number == a.Number);
                fresh.Add(new Account(
                    account?.Id ?? 0,
                    a.Number,
                    account?.Name ?? $"{Brokerage.Questrade.Name} {a.Type} {a.Number}",
                    await GetBalancesAsync(a.Number, accessToken),
                    await GetPositionsAsync(a.Number, accessToken)));
            }

            return fresh;
        }

        async Task<IEnumerable<Balance>> GetBalancesAsync(string accountNumber, AccessToken accessToken)
        {
            var balances = new List<Balance>();
            foreach (var b in (await BalanceApi.FindBalancesAsync(accountNumber, accessToken)).PerCurrencyBalances)
                balances.Add(new Balance(Balance.Cash, b.Cash, await CurrencyReader.ReadByCodeAsync(b.Currency.ToString())));

            return balances;
        }

        async Task<IEnumerable<Position>> GetPositionsAsync(string accountNumber, AccessToken accessToken)
        {
            var positions = new List<Position>();
            foreach (var p in (await PositionApi.FindPositionsAsync(accountNumber, accessToken)).Positions.Where(p => p.CurrentMarketValue.HasValue))
                positions.Add(await GetPositionAsync(p, accessToken));

            return positions;
        }

        async Task<Position> GetPositionAsync(QuestradePosition questradePosition, AccessToken accessToken) =>
            new Position(await GetSymbolAsync(questradePosition.SymbolId, accessToken), questradePosition.CurrentMarketValue.Value);

        async Task<Symbol> GetSymbolAsync(int questradeSymbolId, AccessToken accessToken)
        {
            var symbol = await SymbolReader.ReadByBrokerageReferenceAsync(Brokerage.Questrade.Id, questradeSymbolId.ToString());
            if (symbol != null)
                return symbol;

            var questradeSymbol = await SymbolApi.FindSymbolAsync(questradeSymbolId, accessToken);
            return await SymbolWriter.WriteAsync(
                questradeSymbol.Symbol,
                questradeSymbol.Description,
                questradeSymbol.ListingExchange?.ToString(),
                questradeSymbol.Currency,
                Brokerage.Questrade.Id,
                questradeSymbol.SymbolId.ToString());
        }
    }
}
