using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PortfolioInsight.Configuration;
using PortfolioInsight.Financial;
using Xunit;

namespace PortfolioInsight.ExchangeRatesApi.Tests.Financial
{
    public class CurrencySynchronizer_Should
    {
        [Fact]
        public async Task Synchronize()
        {
            var exchangeRatesApiSettings = new Mock<IExchangeRatesApiSettings>();

            var currencyReader = new Mock<ICurrencyReader>();
            currencyReader.Setup(_ => _.ReadAllAsync())
                .ReturnsAsync(new List<Currency>
                {
                    new Currency("CAD", Rate.Full, DateTime.UtcNow.Date),
                    new Currency("USD", Rate.Full, DateTime.UtcNow.Date)
                });

            var currencyWriter = new Mock<ICurrencyWriter>();
            
            var synchronizer = new CurrencySynchronizer(exchangeRatesApiSettings.Object, currencyReader.Object, currencyWriter.Object);
            await synchronizer.SyncAsync();

            currencyWriter.Verify(_ =>
                _.WriteAsync(It.IsAny<Currency>()),
                Times.Exactly(2));

            currencyWriter.Verify(_ =>
                _.WriteAsync(It.Is<Currency>(c => c.Code == "CAD")),
                Times.Once);

            currencyWriter.Verify(_ =>
                _.WriteAsync(It.Is<Currency>(c => c.Code == "USD" && c.Rate == Rate.Full)),
                Times.Once);
        }
    }
}
