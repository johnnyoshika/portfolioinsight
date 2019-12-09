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
            IAccountReader accountReader,
            IConnectionSynchronizer connectionSynchronizer,
            ICurrencySynchronizer currencySynchronizer,
            ICurrencyReader currencyReader,
            IPortfolioReader portfolioReader,
            IAllocationReader allocationReader,
            IAssetClassReader assetClassReader)
        {
            AuthenticationClient = authenticationClient;
            QuestradeSettings = questradeSettings;
            Tokenizer = tokenizer;
            ConnectionReader = connectionReader;
            AccountReader = accountReader;
            ConnectionSynchronizer = connectionSynchronizer;
            CurrencySynchronizer = currencySynchronizer;
            CurrencyReader = currencyReader;
            PortfolioReader = portfolioReader;
            AllocationReader = allocationReader;
            AssetClassReader = assetClassReader;
        }

        IAuthenticationClient AuthenticationClient { get; }
        IQuestradeSettings QuestradeSettings { get; }
        ITokenizer Tokenizer { get; }
        IConnectionReader ConnectionReader { get; }
        IAccountReader AccountReader { get; }
        IConnectionSynchronizer ConnectionSynchronizer { get; }
        ICurrencySynchronizer CurrencySynchronizer { get; }
        ICurrencyReader CurrencyReader { get; }
        IPortfolioReader PortfolioReader { get; }
        IAllocationReader AllocationReader { get; }
        IAssetClassReader AssetClassReader { get; }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
            return View(await PortfolioReader.ReadByUserIdAsync(user.Id));
        }
             
        [Authorize]
        [HttpGet("portfolios/{id:int}")]
        public async Task<IActionResult> Portfolio(int id)
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
            var accounts = new List<Account>();
            foreach (var connection in await ConnectionReader.ReadByUserIdAsync(user.Id))
                accounts.AddRange(await AccountReader.ReadByConnectionIdAsync(connection.Id));

            var currencies = await CurrencyReader.ReadAllAsync();
            return View(new PortfolioViewModel
            {
                User = user,
                Report = new Report(
                    accounts,
                    await AllocationReader.ReadByPortfolioIdAsync(id),
                    await AssetClassReader.ReadCashByPortfolioIdAsync(id),
                    currencies,
                    currencies.First(c => c.Code == "CAD")
                )
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
    }

    public class PortfolioViewModel
    {
        public User User { get; set; }
        public Report Report { get; set; }
    }
}
