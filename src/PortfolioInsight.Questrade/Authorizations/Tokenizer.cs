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

namespace PortfolioInsight.Authorizations
{
    [Service]
    public class Tokenizer : ITokenizer
    {
        public Tokenizer(IQuestradeSettings questradeSettings, IAuthorizationReader authorizationReader, IAuthorizationWriter authorizationWriter)
        {
            QuestradeSettings = questradeSettings;
            AuthorizationReader = authorizationReader;
            AuthorizationWriter = authorizationWriter;
        }

        IQuestradeSettings QuestradeSettings { get; }
        IAuthorizationReader AuthorizationReader { get; }
        IAuthorizationWriter AuthorizationWriter { get; }

        public async Task<AccessToken> ExchangeAsync(string code, User user, string redirectUrl)
        {
            var token = await FetchTokenAsync($"https://login.questrade.com/oauth2/token?client_id={QuestradeSettings.ConsumerKey}&code={code}&grant_type=authorization_code&redirect_uri={redirectUrl}");
            var accessToken = new AccessToken
            {
                Value = token.AccessToken,
                ApiServer = token.ApiServer
            };
            var accounts = await AccountApi.FindAccountsAsync(accessToken);
            var authorization = await AuthorizationReader.ReadByUserBrokerageAsync(user.Id, Brokerage.Questrade.Id, accounts.UserId);
            if (authorization == null)
                authorization = new Authorization
                {
                    User = user,
                    Brokerage = Brokerage.Questrade,
                    BrokerageUserId = accounts.UserId
                };

            authorization.RefreshToken = token.RefreshToken;

            await AuthorizationWriter.WriteAsync(authorization);
            return accessToken;
        }

        public async Task<AccessToken> RefreshAsync(Authorization authorization)
        {
            var token = await FetchTokenAsync($"https://login.questrade.com/oauth2/token?grant_type=refresh_token&refresh_token={authorization.RefreshToken}");
            authorization.RefreshToken = token.RefreshToken;
            await AuthorizationWriter.WriteAsync(authorization);
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

                    if (response.StatusCode == HttpStatusCode.Redirect && response.Headers.Location.OriginalString.StartsWith("/OAuth2/Errors/GenericError.aspx"))
                        throw new ErrorException("Temporary error fetching access token. Try again later!");

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
