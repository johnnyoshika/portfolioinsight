using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;
using PortfolioInsight.Reports;
using Xunit;

namespace PortfolioInsight.Domain.Tests.Reports
{
    public class Report_Assets_Should
    {
        static readonly Rate CadConversion = (Rate)0.75m;

        static readonly Currency CAD = new Currency("CAD", CadConversion, DateTime.UtcNow.Date);
        static readonly Currency USD = new Currency("USD", Rate.Full, DateTime.UtcNow.Date);

        static readonly AssetClass Cash = new AssetClass(0, Balance.Cash, null);

        static readonly ListingExchange TSX = new ListingExchange("TSX");
        static readonly ListingExchange NYSE = new ListingExchange("NYSE");

        static readonly Symbol XIC = new Symbol(3, "XIC", "", CAD, TSX);
        static readonly Symbol XUS = new Symbol(4, "XUS", "", CAD, TSX);

        [Fact]
        public void List_All_Assets()
        {
            var report = new Report(
                new List<Account>
                {
                    Account(
                        new List<Balance>
                        {
                            new Balance(Balance.Cash, 100, CAD),
                            new Balance(Balance.Cash, 200, USD)
                        },
                        new List<Position>
                        {
                            new Position(XIC, 300),
                            new Position(XUS, 400)
                        }),
                    Account(
                        new List<Balance>
                        {
                            new Balance(Balance.Cash, 500, CAD),
                            new Balance(Balance.Cash, 600, USD)
                        },
                        new List<Position>
                        {
                            new Position(XUS, 700)
                        }),
                    Account(
                        new List<Balance>
                        {
                            new Balance(Balance.Cash, 800, CAD)
                        },
                        new List<Position>
                        {
                            new Position(XIC, 900)
                        })
                },
                new List<Allocation>
                {
                    Allocation(
                        XIC, new AssetClass(1, "CA", null)),
                    Allocation(
                        XUS, new AssetClass(2, "US", null))
                },
                Cash,
                new List<Currency>(),
                CAD
            );

            Assert.Equal(3, report.Assets.Count);
            Assert.Equal("CA", report.Assets.ElementAt(0).AssetClass.Name);
            Assert.Equal((Amount)300 + 900, report.Assets.ElementAt(0).Value);
            Assert.Equal((Rate)
                (
                    ((Amount)(300 + 900)).Value
                    /
                    ((Amount)(100 + 300 + 400 + 500 + 700 + 800 + 900 + (200 + 600) / CadConversion)).Value
                ), report.Assets.ElementAt(0).Proportion);
            Assert.Equal("US", report.Assets.ElementAt(1).AssetClass.Name);
            Assert.Equal((Amount)400 + 700, report.Assets.ElementAt(1).Value);
            Assert.Equal((Rate)
                (
                    ((Amount)(400 + 700)).Value
                    /
                    ((Amount)(100 + 300 + 400 + 500 + 700 + 800 + 900 + (200 + 600) / CadConversion)).Value
                ), report.Assets.ElementAt(1).Proportion);
            Assert.Equal(Balance.Cash, report.Assets.ElementAt(2).AssetClass.Name);
            Assert.Equal(100 + 500 + 800 + (Amount)((200 + 600) / CadConversion), report.Assets.ElementAt(2).Value);
            Assert.Equal((Rate)
                (
                    ((Amount)(100 + 500 + 800 + (200 + 600) / CadConversion)).Value
                    /
                    ((Amount)(100 + 300 + 400 + 500 + 700 + 800 + 900 + (200 + 600) / CadConversion)).Value
                ), report.Assets.ElementAt(2).Proportion);
        }

        Account Account(IEnumerable<Balance> balances, IEnumerable<Position> positions) =>
            new Account(new Random().Next(1, 1000), "", "", balances, positions);

        Allocation Allocation(Symbol symbol, params AllocationProportion[] proportions) =>
            new Allocation(symbol, proportions);

        Allocation Allocation(Symbol symbol, AssetClass assetClass) =>
            Allocation(symbol, new List<AllocationProportion> { new AllocationProportion(assetClass, Rate.Full) }.ToArray());
    }
}
