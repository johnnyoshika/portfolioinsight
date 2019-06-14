using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioInsight.Connections;
using PortfolioInsight.Configuration;
using PortfolioInsight.Exceptions;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;
using PortfolioInsight.Reports;
using PortfolioInsight.Users;
using PortfolioInsight.Web.Http;

namespace PortfolioInsight.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            IAuthenticationClient authenticationClient,
            IQuestradeSettings questradeSettings,
            ITokenizer tokenizer,
            IConnectionReader connectionReader,
            IConnectionSynchronizer connectionSynchronizer,
            ICurrencySynchronizer currencySynchronizer,
            IPortfolioReader portfolioReader,
            IReportReader reportReader,
            IReporter reporter,
            IAllocationReader allocationReader,
            IAssetClassReader assetClassReader,
            ISymbolReader symbolReader,
            IAllocationWriter allocationWriter,
            IAssetClassWriter assetClassWriter)
        {
            AuthenticationClient = authenticationClient;
            QuestradeSettings = questradeSettings;
            Tokenizer = tokenizer;
            ConnectionReader = connectionReader;
            ConnectionSynchronizer = connectionSynchronizer;
            CurrencySynchronizer = currencySynchronizer;
            PortfolioReader = portfolioReader;
            ReportReader = reportReader;
            Reporter = reporter;
            AllocationReader = allocationReader;
            AssetClassReader = assetClassReader;
            SymbolReader = symbolReader;
            AllocationWriter = allocationWriter;
            AssetClassWriter = assetClassWriter;
        }

        IAuthenticationClient AuthenticationClient { get; }
        IQuestradeSettings QuestradeSettings { get; }
        ITokenizer Tokenizer { get; }
        IConnectionReader ConnectionReader { get; }
        IConnectionSynchronizer ConnectionSynchronizer { get; }
        ICurrencySynchronizer CurrencySynchronizer { get; }
        IPortfolioReader PortfolioReader { get; }
        IReportReader ReportReader { get; }
        IReporter Reporter { get; }
        IAllocationReader AllocationReader { get; }
        IAssetClassReader AssetClassReader { get; }
        ISymbolReader SymbolReader { get; }
        IAllocationWriter AllocationWriter { get; }
        IAssetClassWriter AssetClassWriter { get; }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
            return View(new IndexViewModel
            {
                User = user,
                Portfolios = await PortfolioReader.ReadByUserIdAsync(user.Id)
            });
        }
             
        [Authorize]
        [HttpGet("portfolios/{id:int}")]
        public async Task<IActionResult> Portfolio(int id, DateTime? date)
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
            if (!await PortfolioReader.UserOwnsPortfolio(id, user.Id))
                throw new UnauthorizedAccessException();

            var report = date == null ? await Reporter.GenerateAsync(user.Id, id) : await ReportReader.ReadSnapshotAsync(id, date.Value);

            if (report == null)
                return Content("Not found");

            return View(new PortfolioViewModel
            {
                User = user,
                Portfolio = await PortfolioReader.ReadByIdAsync(id),
                Report = report
            });
        }

        [Authorize]
        [HttpGet("questrade/request")]
        public IActionResult QuestradeRequest() =>
            Redirect($"https://login.questrade.com/oauth2/authorize?client_id={QuestradeSettings.ConsumerKey}&response_type=code&redirect_uri={Request.AbsoluteUrl("/questrade/response").UrlEncode()}");

        [Authorize]
        [HttpGet("questrade/response")]
        public async Task<IActionResult> QuestradeResponse(string code)
        {
            try
            {
                await Tokenizer.ExchangeAsync(
                    code,
                    await AuthenticationClient.AuthenticateAsync(HttpContext.Request),
                    Request.AbsoluteUrl("/questrade/response").UrlEncode());

                return Redirect("/");
            }
            catch (ErrorException ex)
            {
                return Content(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("sync")]
        public async Task<IActionResult> Sync()
        {
            try
            {
                var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
                foreach (var connection in await ConnectionReader.ReadByUserIdAsync(user.Id))
                    await ConnectionSynchronizer.SyncAsync(connection);

                await CurrencySynchronizer.SyncAsync();

                return NoContent();
            }
            catch (ErrorException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("allocations")]
        public async Task<IActionResult> Test()
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);

            var allocations = new Allocation[]
            {
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("XEF.TO", "TSX"  ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "INTL", (Rate)0.17m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("XEC.TO", "TSX"  ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "EM", (Rate)0.08m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("XIC.TO", "TSX"  ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "CA", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("XUU.TO", "TSX"  ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "US", (Rate)0.30m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("ITOT",    "ARCA" ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "US", (Rate)0.30m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("ZDB.TO",  "TSX" ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "BOND", (Rate)0.20m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAtListingExchangeAsync("ZAG.TO",  "TSX" ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(user.Id, "BOND", (Rate)0.20m), Rate.Full)})
            };

            foreach (var allocation in allocations)
                await AllocationWriter.WriteAsync(user.Id, allocation);

            return NoContent();
        }
    }
    public class DashboardViewModel
    {
        public User User { get; set; }
    }

    public class IndexViewModel : DashboardViewModel
    {
        public List<Portfolio> Portfolios { get; set; }
    }

    public class PortfolioViewModel : DashboardViewModel
    {
        public Portfolio Portfolio { get; set; }
        public Report Report { get; set; }
    }
}
