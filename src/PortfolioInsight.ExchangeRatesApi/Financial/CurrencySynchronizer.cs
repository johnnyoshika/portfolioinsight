using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortfolioInsight.Exceptions;

namespace PortfolioInsight.Financial
{
    [Service]
    public class CurrencySynchronizer : ICurrencySynchronizer
    {
        public CurrencySynchronizer(ICurrencyReader currencyReader, ICurrencyWriter currencyWriter)
        {
            CurrencyReader = currencyReader;
            CurrencyWriter = currencyWriter;
        }

        ICurrencyReader CurrencyReader { get; }
        ICurrencyWriter CurrencyWriter { get; }

        public async Task SyncAsync()
        {
            var latest = await GetLatest();
            foreach (var c in await CurrencyReader.ReadAllAsync())
                if (latest.Rates.ContainsKey(c.Code))
                    await CurrencyWriter.WriteAsync(new Currency(c.Code, (Rate)(1 / latest.Rates[c.Code]), latest.Date));
        }

        async Task<ExchangeRatesApiLatest> GetLatest()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("https://api.exchangeratesapi.io/latest?base=USD");
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<ExchangeRatesApiLatest>(await response.Content.ReadAsStringAsync());
                }
                catch (HttpRequestException ex)
                {
                    throw new ErrorException(ex.Message);
                }
            }
        }
    }

    class ExchangeRatesApiLatest
    {
        public string Base { get; set; }
        public IDictionary<string, decimal> Rates { get; set; }
        public DateTime Date { get; set; }
    }
}
