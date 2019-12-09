using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortfolioInsight.Connections;

namespace PortfolioInsight.Http
{
    static class BalanceApi
    {
        public static async Task<QuestradeBalances> FindBalancesAsync(string accountNumber, AccessToken accessToken)
        {
            var client = new QuestradeClient(accessToken.Value);
            var data = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"{accessToken.ApiServer}v1/accounts/{accountNumber}/balances"),
                Method = HttpMethod.Get
            });
            return JsonConvert.DeserializeObject<QuestradeBalances>(data);
        }
    }

    class QuestradeBalances
    {
        public List<QuestradeBalance> PerCurrencyBalances { get; set; }
        public List<QuestradeBalance> CombinedBalances { get; set; }
        public List<QuestradeBalance> SodPerCurrencyBalances { get; set; }
        public List<QuestradeBalance> SodCombinedBalances { get; set; }
    }

    class QuestradeBalance
    {
        public QuestradeCurrency Currency { get; set; }
        public decimal Cash { get; set; }
        public decimal MarketValue { get; set; }
        public decimal TotalEquity { get; set; }
        public decimal BuyingPower { get; set; }
        public decimal MaintenanceExcess { get; set; }
        public bool IsRealTime { get; set; }
    }
}
