using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Authorizations;
using PortfolioInsight.Brokerages;
using PortfolioInsight.Financial;
using PortfolioInsight.Http;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class PortfolioSynchronizer : IPortfolioSynchronizer
    {
        public PortfolioSynchronizer(IPortfolioWriter portfolioWriter, ISymbolReader symbolReader, ISymbolWriter symbolWriter, ITokenizer tokenizer)
        {
            PortfolioWriter = portfolioWriter;
            SymbolReader = symbolReader;
            SymbolWriter = symbolWriter;
            Tokenizer = tokenizer;
        }

        IPortfolioWriter PortfolioWriter { get; }
        ISymbolReader SymbolReader { get; }
        ISymbolWriter SymbolWriter { get; }
        ITokenizer Tokenizer { get; }

        public async Task SyncAsync(Authorization authorization) =>
            await PortfolioWriter.WriteAsync(
                new Portfolio(
                    authorization.Id,
                    await GetAccounts(await Tokenizer.RefreshAsync(authorization))));

        async Task<IEnumerable<Account>> GetAccounts(AccessToken accessToken)
        {
            var accounts = new List<Account>();
            foreach (var a in (await AccountApi.FindAccountsAsync(accessToken)).Accounts)
                accounts.Add(new Account(
                    a.Number, 
                    $"{Brokerage.Questrade.Name} {a.Type} {a.Number}",
                    await GetBalancesAsync(a.Number, accessToken),
                    await GetPositionsAsync(a.Number, accessToken)));

            return accounts;
        }

        async Task<IEnumerable<Balance>> GetBalancesAsync(string accountNumber, AccessToken accessToken) =>
            (await BalanceApi.FindBalancesAsync(accountNumber, accessToken)).PerCurrencyBalances
                .Select(b => new Balance("CASH", b.Cash, new Currency(b.Currency.ToString())));

        async Task<IEnumerable<Position>> GetPositionsAsync(string accountNumber, AccessToken accessToken)
        {
            var positions = new List<Position>();
            foreach (var p in (await PositionApi.FindPositionsAsync(accountNumber, accessToken)).Positions)
                positions.Add(await GetPositionAsync(p, accessToken));

            return positions;
        }

        async Task<Position> GetPositionAsync(QuestradePosition questradePosition, AccessToken accessToken) =>
            new Position(await GetSymbolAsync(questradePosition.SymbolId, accessToken), questradePosition.CurrentMarketValue);

        async Task<Symbol> GetSymbolAsync(int questradeSymbolId, AccessToken accessToken)
        {
            var symbol = await SymbolReader.ReadByBrokerageReferenceAsync(Brokerage.Questrade.Id, questradeSymbolId.ToString());
            if (symbol != null)
                return symbol;

            var questradeSymbol = await SymbolApi.FindSymbolAsync(questradeSymbolId, accessToken);
            return await SymbolWriter.WriteAsync(
                Brokerage.Questrade.Id,
                questradeSymbol.SymbolId.ToString(),
                questradeSymbol.Symbol,
                questradeSymbol.Currency,
                questradeSymbol.ListingExchange.ToString());
        }
    }
}
