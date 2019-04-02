using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PortfolioInsight.Configuration;
using PortfolioInsight.Exceptions;

namespace PortfolioInsight.Http
{
    public class QuestradeClient
    {
        public QuestradeClient(string accessToken)
        {
            AccessToken = accessToken;
        }

        string AccessToken { get; }

        void AddAuthorization(HttpRequestHeaders requestHeaders) =>
            requestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        public async Task<string> SendAsync(HttpRequestMessage message)
        {
            AddAuthorization(message.Headers);
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.SendAsync(message);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    throw new ErrorException(ex.Message);
                }
            }
        }
    }
}
