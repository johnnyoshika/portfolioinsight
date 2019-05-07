using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortfolioInsight.Authorizations;

namespace PortfolioInsight.Http
{
    static class PositionApi
    {
        public static async Task<QuestradePositions> FindPositionsAsync(string accountNumber, AccessToken accessToken)
        {
            var client = new QuestradeClient(accessToken.Value);
            var data = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"{accessToken.ApiServer}v1/accounts/{accountNumber}/positions"),
                Method = HttpMethod.Get
            });
            return JsonConvert.DeserializeObject<QuestradePositions>(data);
        }
    }

    class QuestradePositions
    {
        public List<QuestradePosition> Positions { get; set; }
    }

    class QuestradePosition
    {
        public string Symbol { get; set; }
        public int SymbolId { get; set; }
        public decimal OpenQuantity { get; set; }
        public decimal ClosedQuantity { get; set; }
        public decimal? CurrentMarketValue { get; set; } // null means position no longer has value (e.g. sold everything today)
        public decimal CurrentPrice { get; set; }
        public decimal? AverageEntryPrice { get; set; } // null means position no longer has value (e.g. sold everything today)
        public decimal ClosedPnL { get; set; }
        public decimal OpenPnL { get; set; }
        public decimal? TotalCost { get; set; } // null means position no longer has value (e.g. sold everything today)
        public Boolean IsRealTime { get; set; }
        public bool IsUnderReorg { get; set; }
    }
}
