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
    public class Report_BalanceAssets_Should
    {
        static readonly Rate CadConversion = (Rate)0.75m;

        static readonly Currency CAD = new Currency("CAD", CadConversion, DateTime.UtcNow.Date);
        static readonly Currency USD = new Currency("USD", Rate.Full, DateTime.UtcNow.Date);

        [Fact]
        public void Calculate_Assets_In_USD_Currency()
        {
            var report = new Report(
                new List<Account>
                {
                    Account(
                        new Balance(Balance.Cash, 100, USD))
                },
                new List<Allocation>(),
                new AssetClass(0, Balance.Cash, null),
                new List<Currency>(),
                USD
            );

            Assert.Equal(1, report.BalanceAssets.Count);
            Assert.Equal(Balance.Cash, report.BalanceAssets.First().AssetClass.Name);
            Assert.Equal(100, report.BalanceAssets.First().Value);
            Assert.Equal(Rate.Full, report.BalanceAssets.First().Proportion);
        }

        [Fact]
        public void Calculate_Assets_In_CAD_Currency()
        {
            var report = new Report(
                new List<Account>
                {
                    Account(
                        new Balance(Balance.Cash, 100, USD))
                },
                new List<Allocation>(),
                new AssetClass(0, Balance.Cash, null),
                new List<Currency>(),
                CAD
            );

            Assert.Equal(1, report.BalanceAssets.Count);
            Assert.Equal(Balance.Cash, report.BalanceAssets.First().AssetClass.Name);
            Assert.Equal((Amount)(100 / CadConversion), report.BalanceAssets.First().Value);
            Assert.Equal(Rate.Full, report.BalanceAssets.First().Proportion);
        }

        [Fact]
        public void Convert_USD_To_CAD()
        {
            var report = new Report(
                new List<Account>
                {
                    Account(
                        new Balance(Balance.Cash, 100, USD))
                },
                new List<Allocation>(),
                new AssetClass(0, Balance.Cash, null),
                new List<Currency>(),
                CAD
            );

            Assert.Equal(1, report.BalanceAssets.Count);
            Assert.Equal(Balance.Cash, report.BalanceAssets.First().AssetClass.Name);
            Assert.Equal((Amount)(100 / CadConversion), report.BalanceAssets.First().Value);
            Assert.Equal(Rate.Full, report.BalanceAssets.First().Proportion);
        }

        [Fact]
        public void Be_Empty_If_No_Cash_Balance()
        {
            var report = new Report(
                new List<Account>
                {
                    Account(
                        new Balance("Unknown", 100, CAD))
                },
                new List<Allocation>(),
                new AssetClass(0, Balance.Cash, null),
                new List<Currency>(),
                CAD
            );

            Assert.Empty(report.BalanceAssets);
        }


        [Fact]
        public void Combine_All_Cash_Balances_And_Currencies()
        {
            var report = new Report(
                new List<Account>
                {
                    Account(
                        new Balance(Balance.Cash, 100, CAD),
                        new Balance(Balance.Cash, 200, CAD),
                        new Balance(Balance.Cash, 300, USD),
                        new Balance(Balance.Cash, 400, USD)),
                    Account(
                        new Balance("Unknown", 500, CAD),
                        new Balance(Balance.Cash, 600, CAD),
                        new Balance(Balance.Cash, 700, USD),
                        new Balance(Balance.Cash, 800, USD))
                },
                new List<Allocation>(),
                new AssetClass(0, Balance.Cash, null),
                new List<Currency>(),
                CAD
            );

            Assert.Equal(1, report.BalanceAssets.Count);
            Assert.Equal(Balance.Cash, report.BalanceAssets.First().AssetClass.Name);
            Assert.Equal(100 + 200 + 600 + (Amount)((300 + 400 + 700 + 800) / CadConversion), report.BalanceAssets.First().Value);

            // need to cast to Amount, which rounds to 2 decimal places, otherwise, we'll get rounding error
            Assert.Equal((Rate)
                (
                    ((Amount)(100m + 200 + 600 + ((300 + 400 + 700 + 800) / CadConversion))).Value
                    /
                    ((Amount)(100 + 200 + 500 + 600 + ((300 + 400 + 700 + 800) / CadConversion))).Value
                ), report.BalanceAssets.First().Proportion);
        }

        Account Account(params Balance[] balances) =>
            new Account(new Random().Next(1, 1000), "", "", balances, new List<Position>());
    }
}
