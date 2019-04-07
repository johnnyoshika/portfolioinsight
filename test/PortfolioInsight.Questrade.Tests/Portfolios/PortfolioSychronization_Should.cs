using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PortfolioInsight.Authorizations;
using PortfolioInsight.Configuration;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;
using Xunit;

using static System.IO.File;
using static System.IO.Path;

namespace PortfolioInsight.Questrade.Tests.Portfolios
{
    public class PortfolioSychronization_Should
    {
        [Fact]
        public async Task Synchronize()
        {
            // Manually generate from here: https://login.questrade.com/APIAccess/UserApps.aspx
            string refreshToken = "";

            var tokenizer = new Mock<ITokenizer>();
            tokenizer.Setup(_ => _.RefreshAsync(It.IsAny<Authorization>()))
                .ReturnsAsync(await RefreshTokenAsync(refreshToken));

            var portfolioReader = new Mock<IPortfolioReader>();
            portfolioReader.Setup(_ => _.ReadByAuthorizationIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Portfolio(0, new List<Account>()));

            var symbolReader = new Mock<ISymbolReader>();
            symbolReader.Setup(_ => _.ReadByBrokerageReferenceAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => 
                    new Symbol(1, "XIC", "TSX Composite ETF", new Currency("CAD"), new ListingExchange("TSX")));

            var symbolWriter = new Mock<ISymbolWriter>();

            Portfolio portfolio = null;
            var portfolioWriter = new Mock<IPortfolioWriter>();
            portfolioWriter.Setup(_ => _.WriteAsync(It.IsAny<Portfolio>()))
                .Callback<Portfolio>(p =>
                {
                    portfolio = p;
                })
                .Returns(Task.CompletedTask);

            var synchronizer = new PortfolioSynchronizer(portfolioReader.Object, portfolioWriter.Object, symbolReader.Object, symbolWriter.Object, tokenizer.Object);
            await synchronizer.SyncAsync(new Authorization());

            Assert.NotNull(portfolio);
        }

        /// <summary>
        /// Manually generate refresh token from here: https://login.questrade.com/APIAccess/UserApps.aspx
        /// The first time that refresh token is used, this method retrieves an access token from Questrade's API using that refresh token.
        /// It then stores the next refresh token in the 'keys' folder with the following pattern:
        /// 
        /// file name: original refresh token
        /// file content: next refresh token
        /// 
        /// All subsequent use of the refresh token will retrieve the next refresh token from the 'keys' folder.
        /// </summary>
        async Task<AccessToken> RefreshTokenAsync(string refreshToken)
        {
            string project = "PortfolioInsight.Questrade.Tests";
            string running = GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string directory = Combine(running.Substring(0, running.IndexOf(project) + project.Length), "keys");
            string file = Combine(directory, $"{refreshToken}.txt");

            var questradeSettings = new Mock<IQuestradeSettings>();
            var authorizationReader = new Mock<IAuthorizationReader>();

            Authorization authorization = null;
            var authorizationWriter = new Mock<IAuthorizationWriter>();
            authorizationWriter.Setup(_ => _.WriteAsync(It.IsAny<Authorization>()))
                .Callback<Authorization>(a =>
                {
                    authorization = a;
                })
                .Returns(Task.CompletedTask);

            var tokenizer = new Tokenizer(questradeSettings.Object, authorizationReader.Object, authorizationWriter.Object);
            var accessToken = await tokenizer.RefreshAsync(
                new Authorization
                {
                    RefreshToken = Exists(file)
                        ? await ReadAllTextAsync(file)
                        : refreshToken
                });

            WriteAllText(file, authorization.RefreshToken);
            return accessToken;
        }
    }
}
