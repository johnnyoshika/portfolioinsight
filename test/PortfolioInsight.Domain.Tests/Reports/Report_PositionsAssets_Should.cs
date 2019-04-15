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
    public class Report_PositionsAssets_Should
    {
        static readonly Rate CadConversion = (Rate)0.75m;

        static readonly Currency CAD = new Currency("CAD", CadConversion);
        static readonly Currency USD = new Currency("USD", Rate.Full);

        static readonly AssetClass Cash = new AssetClass(0, Balance.Cash, null);

        static readonly ListingExchange TSX = new ListingExchange("TSX");
        static readonly ListingExchange NYSE = new ListingExchange("NYSE");

        static readonly Symbol ZAG = new Symbol(1, "ZAG", "", CAD, TSX);
        static readonly Symbol ZDB = new Symbol(2, "ZDB", "", CAD, TSX);

        static readonly Symbol XIC = new Symbol(3, "XIC", "", CAD, TSX);
        static readonly Symbol XUS = new Symbol(4, "XUS", "", CAD, TSX);
        static readonly Symbol XUU = new Symbol(5, "XUU", "", CAD, TSX);
        static readonly Symbol XAW = new Symbol(6, "XAW", "", CAD, TSX);

        static readonly Symbol VOO = new Symbol(7, "VOO", "", USD, NYSE);
        static readonly Symbol VTI = new Symbol(8, "VTI", "", USD, NYSE);

        [Fact]
        public void Calculate_Assets_In_USD_Currency()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(VOO, 100))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        VOO, new AssetClass(1, "US", null))
                },
                Cash,
                USD
            );

            Assert.Equal(1, report.PositionAssets.Count);
            Assert.Equal("US", report.PositionAssets.First().AssetClass.Name);
            Assert.Equal(100, report.PositionAssets.First().Value);
            Assert.Equal(Rate.Full, report.PositionAssets.First().Proportion);
        }

        [Fact]
        public void Calculate_Assets_In_CAD_Currency()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XIC, 100))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XIC, new AssetClass(1, "CA", null))
                },
                Cash,
                CAD
            );

            Assert.Equal(1, report.PositionAssets.Count);
            Assert.Equal("CA", report.PositionAssets.First().AssetClass.Name);
            Assert.Equal(100, report.PositionAssets.First().Value);
            Assert.Equal(Rate.Full, report.PositionAssets.First().Proportion);
        }

        [Fact]
        public void Convert_USD_To_CAD()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XUS, 100),
                            new Position(VOO, 200))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XUS, new AssetClass(1, "US", null)),
                    Allocation(
                        VOO, new AssetClass(1, "US", null))
                },
                Cash,
                CAD
            );

            Assert.Equal(1, report.PositionAssets.Count);
            Assert.Equal("US", report.PositionAssets.First().AssetClass.Name);
            Assert.Equal((Amount)(100 + 200 / CadConversion), report.PositionAssets.First().Value);
            Assert.Equal(Rate.Full, report.PositionAssets.First().Proportion);
        }

        [Fact]
        public void Report_Unclassified_Symbol_As_AssetClass_Unknown()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XIC, 100),
                            new Position(ZAG, 200))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XIC, new AssetClass(1, "CA", null))
                },
                Cash,
                CAD
            );

            Assert.Equal(2, report.PositionAssets.Count);
            Assert.Equal("CA", report.PositionAssets.ElementAt(0).AssetClass.Name);
            Assert.Equal(100, report.PositionAssets.ElementAt(0).Value);
            Assert.Equal((Rate)(100m / 300), report.PositionAssets.ElementAt(0).Proportion);
            Assert.Equal(AssetClass.Unknown, report.PositionAssets.ElementAt(1).AssetClass);
            Assert.Equal(200, report.PositionAssets.ElementAt(1).Value);
            Assert.Equal((Rate)(200m / 300), report.PositionAssets.ElementAt(1).Proportion);
        }

        [Fact]
        public void Calculate_Multi_Proportional_Allocations()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XAW, 100))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XAW,
                        new AllocationProportion(new AssetClass(1, "US", null), (Rate)0.55m),
                        new AllocationProportion(new AssetClass(2, "INTL", null), (Rate)0.325m),
                        new AllocationProportion(new AssetClass(3, "EM", null), (Rate)0.125m))
                },
                Cash,
                CAD
            );

            Assert.Equal(3, report.PositionAssets.Count);
            Assert.Equal("US", report.PositionAssets.ElementAt(0).AssetClass.Name);
            Assert.Equal(55, report.PositionAssets.ElementAt(0).Value);
            Assert.Equal((Rate).55m, report.PositionAssets.ElementAt(0).Proportion);
            Assert.Equal("INTL", report.PositionAssets.ElementAt(1).AssetClass.Name);
            Assert.Equal((Amount)32.5m, report.PositionAssets.ElementAt(1).Value);
            Assert.Equal((Rate).325m, report.PositionAssets.ElementAt(1).Proportion);
            Assert.Equal("EM", report.PositionAssets.ElementAt(2).AssetClass.Name);
            Assert.Equal((Amount)12.5m, report.PositionAssets.ElementAt(2).Value);
            Assert.Equal((Rate).125m, report.PositionAssets.ElementAt(2).Proportion);
        }

        [Fact]
        public void Aggregate_Multiple_Portfolios_And_Accounts()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XIC, 100),
                            new Position(ZAG, 200)),
                        Account(
                            new Position(XIC, 100),
                            new Position(ZAG, 200))
                    }),
                    new Portfolio(2, new List<Account>
                    {
                        Account(
                            new Position(XIC, 100),
                            new Position(ZAG, 200)),
                        Account(
                            new Position(XIC, 100),
                            new Position(ZAG, 200))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XIC, new AssetClass(1, "CA", null)),
                    Allocation(
                        ZAG, new AssetClass(2, "BOND", null))
                },
                Cash,
                CAD
            );

            Assert.Equal(2, report.PositionAssets.Count);
            Assert.Equal("CA", report.PositionAssets.ElementAt(0).AssetClass.Name);
            Assert.Equal(400, report.PositionAssets.ElementAt(0).Value);
            Assert.Equal((Rate)(400m / 1200), report.PositionAssets.ElementAt(0).Proportion);
            Assert.Equal("BOND", report.PositionAssets.ElementAt(1).AssetClass.Name);
            Assert.Equal(800, report.PositionAssets.ElementAt(1).Value);
            Assert.Equal((Rate)(800m / 1200), report.PositionAssets.ElementAt(1).Proportion);
        }

        [Fact]
        public void Combine_Same_Asset_Classes()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XUS, 300),
                            new Position(XUU, 200),
                            new Position(XAW, 100))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XUS, new AssetClass(1, "US", null)),
                    Allocation(
                        XUU, new AssetClass(1, "US", null)),
                    Allocation(
                        XAW,
                        new AllocationProportion(new AssetClass(1, "US", null), (Rate)0.55m),
                        new AllocationProportion(new AssetClass(2, "INTL", null), (Rate)0.325m),
                        new AllocationProportion(new AssetClass(3, "EM", null), (Rate)0.125m))
                },
                Cash,
                CAD
            );

            Assert.Equal(3, report.PositionAssets.Count);
            Assert.Equal("US", report.PositionAssets.ElementAt(0).AssetClass.Name);
            Assert.Equal(300 + 200 + 55, report.PositionAssets.ElementAt(0).Value);
            Assert.Equal((Rate)((300m + 200 + 55) / (300 + 200 + 100)), report.PositionAssets.ElementAt(0).Proportion);
            Assert.Equal("INTL", report.PositionAssets.ElementAt(1).AssetClass.Name);
            Assert.Equal((Amount)32.5m, report.PositionAssets.ElementAt(1).Value);
            Assert.Equal((Rate)(32.5m / (300 + 200 + 100)), report.PositionAssets.ElementAt(1).Proportion);
            Assert.Equal("EM", report.PositionAssets.ElementAt(2).AssetClass.Name);
            Assert.Equal((Amount)12.5m, report.PositionAssets.ElementAt(2).Value);
            Assert.Equal((Rate)(12.5m / (300 + 200 + 100)), report.PositionAssets.ElementAt(2).Proportion);
        }

        [Fact]
        public void Convert_Currencies_Categorizes_Unknown_And_Aggregates()
        {
            var report = new Report(
                new List<Portfolio>
                {
                    new Portfolio(1, new List<Account>
                    {
                        Account(
                            new Position(XUS, 300),
                            new Position(XUU, 200),
                            new Position(XAW, 100),
                            new Position(XIC, 400),
                            new Position(ZAG, 500),
                            new Position(ZDB, 600)),
                        Account(
                            new Position(VOO, 700))
                    }),
                    new Portfolio(2, new List<Account>
                    {
                        Account(
                            new Position(VTI, 800))
                    })
                },
                new List<Allocation>
                {
                    Allocation(
                        XUS, new AssetClass(1, "US", null)),
                    Allocation(
                        XUU, new AssetClass(1, "US", null)),
                    Allocation(
                        XAW,
                        new AllocationProportion(new AssetClass(1, "US", null), (Rate)0.55m),
                        new AllocationProportion(new AssetClass(2, "INTL", null), (Rate)0.325m),
                        new AllocationProportion(new AssetClass(3, "EM", null), (Rate)0.125m)),
                    Allocation(
                        ZAG, new AssetClass(4, "BOND", null)),
                    Allocation(
                        VOO, new AssetClass(1, "US", null)),
                    Allocation(
                        VTI, new AssetClass(1, "US", null))
                },
                Cash,
                CAD
            );

            Assert.Equal(5, report.PositionAssets.Count);
            Assert.Equal("US", report.PositionAssets.ElementAt(0).AssetClass.Name);
            Assert.Equal((Amount)300 + 200 + 55 + (700 + 800) / CadConversion, report.PositionAssets.ElementAt(0).Value);
            Assert.Equal((Rate)((300 + 200 + 55 + 700 / CadConversion + 800 / CadConversion) / (300m + 200 + 100 + 400 + 500 + 600 + (700 + 800) / CadConversion)), report.PositionAssets.ElementAt(0).Proportion);
            Assert.Equal("INTL", report.PositionAssets.ElementAt(1).AssetClass.Name);
            Assert.Equal((Amount)32.5m, report.PositionAssets.ElementAt(1).Value);
            Assert.Equal((Rate)(32.5m / (300m + 200 + 100 + 400 + 500 + 600 + (700 + 800) / CadConversion)), report.PositionAssets.ElementAt(1).Proportion);
            Assert.Equal("EM", report.PositionAssets.ElementAt(2).AssetClass.Name);
            Assert.Equal((Amount)12.5m, report.PositionAssets.ElementAt(2).Value);
            Assert.Equal((Rate)(12.5m / (300m + 200 + 100 + 400 + 500 + 600 + (700 + 800) / CadConversion)), report.PositionAssets.ElementAt(2).Proportion);
            Assert.Equal(AssetClass.Unknown, report.PositionAssets.ElementAt(3).AssetClass);
            Assert.Equal(1000, report.PositionAssets.ElementAt(3).Value);
            Assert.Equal((Rate)(1000m / (300m + 200 + 100 + 400 + 500 + 600 + (700 + 800) / CadConversion)), report.PositionAssets.ElementAt(3).Proportion);
            Assert.Equal("BOND", report.PositionAssets.ElementAt(4).AssetClass.Name);
            Assert.Equal(500, report.PositionAssets.ElementAt(4).Value);
            Assert.Equal((Rate)(500m / (300m + 200 + 100 + 400 + 500 + 600 + (700 + 800) / CadConversion)), report.PositionAssets.ElementAt(4).Proportion);
        }

        Account Account(params Position[] positions) =>
            new Account(new Random().Next(1, 1000), "", "", new List<Balance>(), positions);

        Allocation Allocation(Symbol symbol, params AllocationProportion[] proportions) =>
            new Allocation(symbol, proportions);

        Allocation Allocation(Symbol symbol, AssetClass assetClass) =>
            Allocation(symbol, new List<AllocationProportion> { new AllocationProportion(assetClass, Rate.Full) }.ToArray());
    }
}
