using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortfolioInsight.Connections;

namespace PortfolioInsight.Http
{
    static class AccountApi
    {
        public static async Task<QuestradeAccounts> FindAccountsAsync(AccessToken accessToken)
        {
            var client = new QuestradeClient(accessToken.Value);
            var data = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"{accessToken.ApiServer}v1/accounts"),
                Method = HttpMethod.Get
            });
            return JsonConvert.DeserializeObject<QuestradeAccounts>(data);
        }
    }


    class QuestradeAccounts
    {
        public List<QuestradeAccount> Accounts { get; set; }
        public string UserId { get; set; }
    }

    class QuestradeAccount
    {
        public QuestradeAccountType Type { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsBilling { get; set; }
        public string ClientAccountType { get; set; }
    }
}
