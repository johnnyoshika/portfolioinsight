using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortfolioInsight.Brokerages;
using PortfolioInsight.Configuration;
using PortfolioInsight.Exceptions;
using PortfolioInsight.Http;
using PortfolioInsight.Users;

namespace PortfolioInsight.Connections
{
    [Service]
    public class Tokenizer : ITokenizer
    {
        public Tokenizer(IQuestradeSettings questradeSettings, IConnectionReader connectionReader, IConnectionWriter connectionWriter)
        {
            QuestradeSettings = questradeSettings;
            ConnectionReader = connectionReader;
            ConnectionWriter = connectionWriter;
        }

        IQuestradeSettings QuestradeSettings { get; }
        IConnectionReader ConnectionReader { get; }
        IConnectionWriter ConnectionWriter { get; }

        public async Task<AccessToken> ExchangeAsync(string code, User user, string redirectUrl)
        {
            var token = await FetchTokenAsync($"https://login.questrade.com/oauth2/token?client_id={QuestradeSettings.ConsumerKey}&code={code}&grant_type=authorization_code&redirect_uri={redirectUrl}");
            var accessToken = new AccessToken
            {
                Value = token.AccessToken,
                ApiServer = token.ApiServer
            };
            var accounts = await AccountApi.FindAccountsAsync(accessToken);
            var connection = await ConnectionReader.ReadByUserBrokerageAsync(user.Id, Brokerage.Questrade.Id, accounts.UserId);
            if (connection == null)
                connection = new Connection
                {
                    User = user,
                    Brokerage = Brokerage.Questrade,
                    BrokerageUserId = accounts.UserId
                };

            connection.RefreshToken = token.RefreshToken;

            await ConnectionWriter.WriteAsync(connection);
            return accessToken;
        }

        public async Task<AccessToken> RefreshAsync(Connection connection)
        {
            var token = await FetchTokenAsync($"https://login.questrade.com/oauth2/token?grant_type=refresh_token&refresh_token={connection.RefreshToken}");
            connection.RefreshToken = token.RefreshToken;
            await ConnectionWriter.WriteAsync(connection);
            return new AccessToken
            {
                Value = token.AccessToken,
                ApiServer = token.ApiServer
            };
        }

        async Task<QuestradeToken> FetchTokenAsync(string url)
        {
            using (var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false
            }))
            {
                try
                {
                    var response = await client.PostAsync(url, null);

                    if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        if (response.Headers.Location.OriginalString.StartsWith("/OAuth2/Errors/GenericError.aspx"))
                            throw new ErrorException("Temporary error fetching access token. Try again later!");

                        // Although the redirect URL indicates access-denied, on 2019-06-10 it was just temporary API auth downtime
                        if (response.Headers.Location.OriginalString.Contains("access-denied"))
                            throw new ErrorException("Refresh token denied. Please re-authorize!");
                    }

                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<QuestradeToken>(data);
                }
                catch (HttpRequestException ex)
                {
                    throw new ErrorException(ex.Message);
                }
            }
        }
    }

    class QuestradeToken
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("api_server")] public string ApiServer { get; set; }
    }
}
