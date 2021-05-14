using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PortfolioInsight.Configuration
{
    [Service]
    public class Settings : 
        IQuestradeSettings,
        IExchangeRatesApiSettings
    {
        public Settings(IOptions<ConfigSettings> configSettings)
        {
            ConfigSettings = configSettings.Value;
        }

        ConfigSettings ConfigSettings { get; }

        string IQuestradeSettings.ConsumerKey => ConfigSettings.QuestradeConsumerKey;

        string IExchangeRatesApiSettings.AccessKey => ConfigSettings.ExchangeRatesApiAccessKey;
    }
}
