using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortfolioInsight.Authorizations;

namespace PortfolioInsight.Http
{
    static class SymbolApi
    {
        public static async Task<QuestradeSymbol> FindSymbolAsync(int id, AccessToken accessToken) =>
            await SingleAsync($"/{id}", accessToken);

        public static async Task<QuestradeSymbol> FindSymbolAsync(string name, AccessToken accessToken) =>
            await SingleAsync($"?names={name}", accessToken);

        public static async Task<QuestradeSymbols> FindSymbolsAsync(int[] ids, AccessToken accessToken) =>
            await FindAsync($"?ids={string.Join(",", ids).UrlEncode()}", accessToken);

        public static async Task<QuestradeSymbols> FindSymbolsAsync(string[] names, AccessToken accessToken) =>
            await FindAsync($"?names={string.Join(",", names).UrlEncode()}", accessToken);

        static async Task<QuestradeSymbol> SingleAsync(string filter, AccessToken accessToken)
        {
            var symbols = await FindAsync(filter, accessToken);
            if (!symbols.Symbols.Any())
                return null;

            if (symbols.Symbols.Count() > 1)
                throw new NotSupportedException($"Single symbol lookup yielded {symbols.Symbols.Count()} results.");

            return symbols.Symbols.Single();
        }

        static async Task<QuestradeSymbols> FindAsync(string filter, AccessToken accessToken)
        {
            var client = new QuestradeClient(accessToken.Value);
            var data = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri($"{accessToken.ApiServer}v1/symbols{filter}"),
                Method = HttpMethod.Get
            });
            return JsonConvert.DeserializeObject<QuestradeSymbols>(data);
        }
    }

    class QuestradeSymbols
    {
        public List<QuestradeSymbol> Symbols { get; set; }
    }

    class QuestradeSymbol
    {
        // Does not include the full list
        public string Symbol { get; set; }
        public int SymbolId { get; set; }
        public decimal? PrevDayClosePrice { get; set; }
        public decimal? HighPrice52 { get; set; }
        public decimal? LowPrice52 { get; set; }
        public int? OutstandingShares { get; set; }
        public decimal? Eps { get; set; }
        public decimal? PE { get; set; }
        public decimal? Dividend { get; set; }
        public decimal? Yield { get; set; }
        public DateTime? ExDate { get; set; }
        public decimal? MarketCap { get; set; }
        public QuestradeListingExchange? ListingExchange { get; set; } // Seems to be empty for mutual funds (e.g. RBF558)
        public string Description { get; set; }
        public QuestradeSecurityType SecurityType { get; set; }
        public DateTime? DividendDate { get; set; }
        public string Currency { get; set; } // Not sure why their docs list this as a string and not enum
        public string IndustrySector { get; set; }
        public string IndustryGroup { get; set; }
        public string IndustrySubGroup { get; set; }
    }
}
