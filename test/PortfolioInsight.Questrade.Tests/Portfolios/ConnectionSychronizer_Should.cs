using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PortfolioInsight.Connections;
using PortfolioInsight.Configuration;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;
using Xunit;

using static System.IO.File;
using static System.IO.Path;

namespace PortfolioInsight.Questrade.Tests.Portfolios
{
    public class ConnectionSychronizer_Should
    {
        public string KeysDirectory {
            get
            {
                string project = "PortfolioInsight.Questrade.Tests";
                string running = GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Combine(running.Substring(0, running.IndexOf(project) + project.Length), "keys");
            }
        }


        [Fact]
        public async Task Synchronize()
        {
            var tokenizer = new Mock<ITokenizer>();
            tokenizer.Setup(_ => _.RefreshAsync(It.IsAny<Connection>()))
                .ReturnsAsync(await RefreshTokenAsync());

            var accountReader = new Mock<IAccountReader>();
            accountReader.Setup(_ => _.ReadByConnectionIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Account>());

            var symbolReader = new Mock<ISymbolReader>();
            symbolReader.Setup(_ => _.ReadByBrokerageReferenceAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => 
                    new Symbol(1, "XIC", "TSX Composite ETF", new Currency("CAD", (Rate)0.75m, DateTime.UtcNow.Date), new ListingExchange("TSX")));

            var symbolWriter = new Mock<ISymbolWriter>();

            var currencyReader = new Mock<ICurrencyReader>();
            currencyReader.Setup(_ => _.ReadByCodeAsync("CAD"))
                .ReturnsAsync(new Currency("CAD", Rate.Full, DateTime.Now));
            currencyReader.Setup(_ => _.ReadByCodeAsync("USD"))
                .ReturnsAsync(new Currency("USD", (Rate)0.75m, DateTime.Now));

            int? connectionId = null;
            List<Account> accounts = null;
            var accountWriter = new Mock<IAccountWriter>();
            accountWriter.Setup(_ => _.WriteAsync(It.IsAny<int>(), It.IsAny<List<Account>>()))
                .Callback<int, List<Account>>((i, a) =>
                {
                    connectionId = i;
                    accounts = a;
                })
                .Returns(Task.CompletedTask);

            var synchronizer = new ConnectionSynchronizer(
                accountReader.Object,
                accountWriter.Object,
                symbolReader.Object,
                symbolWriter.Object,
                currencyReader.Object,
                tokenizer.Object);
            
            await synchronizer.SyncAsync(new Connection());

            Assert.NotNull(connectionId);
            Assert.NotNull(accounts);
        }

        /// <summary>
        /// Manually generate refresh token from here: https://login.questrade.com/APIAccess/UserApps.aspx
        /// and save it into the keys directory as 'token.txt' with only the refresh token as the content.
        /// The first time that refresh token is used, this method retrieves an access token from Questrade's API using that refresh token.
        /// It then stores the next refresh token in the 'keys' folder with the following pattern:
        /// 
        /// file name: original refresh token
        /// file content: next refresh token
        /// 
        /// All subsequent use of the refresh token will retrieve the next refresh token from the 'keys' folder.
        /// </summary>
        async Task<AccessToken> RefreshTokenAsync()
        {
            string tokenFileName = "token.txt";
            string tokenFile = Combine(KeysDirectory, tokenFileName);
            if (!Exists(tokenFile))
                throw new InvalidOperationException($"" +
                    $"'{tokenFileName}' missing. " +
                    $"Manually generate it from https://login.questrade.com/APIAccess/UserApps.aspx " +
                    $"and save it in 'keys' directory.");

            string refreshToken = await ReadAllTextAsync(tokenFile);
            string nextTokenFile = Combine(KeysDirectory, $"{refreshToken}.txt");

            var questradeSettings = new Mock<IQuestradeSettings>();
            var connectionReader = new Mock<IConnectionReader>();

            Connection connection = null;
            var connectionWriter = new Mock<IConnectionWriter>();
            connectionWriter.Setup(_ => _.WriteAsync(It.IsAny<Connection>()))
                .Callback<Connection>(c =>
                {
                    connection = c;
                })
                .Returns(Task.CompletedTask);

            var tokenizer = new Tokenizer(questradeSettings.Object, connectionReader.Object, connectionWriter.Object);
            var accessToken = await tokenizer.RefreshAsync(
                new Connection
                {
                    RefreshToken = Exists(nextTokenFile)
                        ? await ReadAllTextAsync(nextTokenFile)
                        : refreshToken
                });

            WriteAllText(nextTokenFile, connection.RefreshToken);
            return accessToken;
        }
    }
}
