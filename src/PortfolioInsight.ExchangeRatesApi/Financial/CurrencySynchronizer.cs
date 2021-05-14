using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortfolioInsight.Configuration;
using PortfolioInsight.Exceptions;

namespace PortfolioInsight.Financial
{
    [Service]
    public class CurrencySynchronizer : ICurrencySynchronizer
    {
        public CurrencySynchronizer(IExchangeRatesApiSettings settings, ICurrencyReader currencyReader, ICurrencyWriter currencyWriter)
        {
            Settings = settings;
            CurrencyReader = currencyReader;
            CurrencyWriter = currencyWriter;
        }

        IExchangeRatesApiSettings Settings { get; }
        ICurrencyReader CurrencyReader { get; }
        ICurrencyWriter CurrencyWriter { get; }

        public async Task SyncAsync()
        {
            var latest = await GetLatest();
            foreach (var c in await CurrencyReader.ReadAllAsync())
                if (latest.Rates.ContainsKey(c.Code) && latest.Rates.ContainsKey("USD"))
                    await CurrencyWriter.WriteAsync(new Currency(c.Code, (Rate)(latest.Rates["USD"] / latest.Rates[c.Code]), latest.Date));
        }

        async Task<ExchangeRatesApiLatest> GetLatest()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"http://api.exchangeratesapi.io/v1/latest?access_key={Settings.AccessKey}&format=1");
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
