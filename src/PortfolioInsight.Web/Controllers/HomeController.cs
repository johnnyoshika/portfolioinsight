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
using PortfolioInsight.Security;
using PortfolioInsight.Users;
using PortfolioInsight.Web.Http;

namespace PortfolioInsight.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            IUserReader userReader,
            IIdentityReader identityReader,
            ISignInClient signInClient,
            ISignOutClient signOutClient,
            IAuthenticationClient authenticationClient,
            IQuestradeSettings questradeSettings,
            ITokenizer tokenizer,
            IConnectionReader connectionReader,
            IPortfolioReader portfolioReader,
            IPortfolioSynchronizer portfolioSynchronizer,
            ICurrencySynchronizer currencySynchronizer,
            ICurrencyReader currencyReader,
            IAllocationReader allocationReader,
            IAssetClassReader assetClassReader)
        {
            UserReader = userReader;
            IdentityReader = identityReader;
            SignInClient = signInClient;
            SignOutClient = signOutClient;
            AuthenticationClient = authenticationClient;
            QuestradeSettings = questradeSettings;
            Tokenizer = tokenizer;
            ConnectionReader = connectionReader;
            PortfolioReader = portfolioReader;
            PortfolioSynchronizer = portfolioSynchronizer;
            CurrencySynchronizer = currencySynchronizer;
            CurrencyReader = currencyReader;
            AllocationReader = allocationReader;
            AssetClassReader = assetClassReader;
        }

        IUserReader UserReader { get; }
        IIdentityReader IdentityReader { get; }
        ISignInClient SignInClient { get; }
        ISignOutClient SignOutClient { get; }
        IAuthenticationClient AuthenticationClient { get; }
        IQuestradeSettings QuestradeSettings { get; }
        ITokenizer Tokenizer { get; }
        IConnectionReader ConnectionReader { get; }
        IPortfolioReader PortfolioReader { get; }
        IPortfolioSynchronizer PortfolioSynchronizer { get; }
        ICurrencySynchronizer CurrencySynchronizer { get; }
        ICurrencyReader CurrencyReader { get; }
        IAllocationReader AllocationReader { get; }
        IAssetClassReader AssetClassReader { get; }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
            var portfolios = new List<Portfolio>();
            foreach (var connection in await ConnectionReader.ReadByUserAsync(user.Id))
                portfolios.Add(await PortfolioReader.ReadByConnectionIdAsync(connection.Id));

            var currencies = await CurrencyReader.ReadAllAsync();
            return View(new DashboardViewModel
            {
                User = user,
                Report = new Report(
                    portfolios,
                    await AllocationReader.ReadByUserIdAsync(user.Id),
                    await AssetClassReader.ReadCashByUserIdAsync(user.Id),
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

        [HttpGet("account/login")]
        public IActionResult Login() => View();

        [HttpPost("account/login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var user = await IdentityReader.AuthenticateAsync(email, password);
                await SignInClient.SignInAsync(HttpContext, user, true);
                return Redirect("/");
            }
            catch (AuthenticationException)
            {
                return Content("Invalid credentials!");
            }
        }

        [HttpGet("account/logout")]
        public async Task<IActionResult> Logout()
        {
            await SignOutClient.SignOutAsync(HttpContext);
            return Redirect("/account/login");
        }

        [Authorize]
        [HttpPut("sync")]
        public async Task<IActionResult> Sync()
        {
            try
            {
                var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
                foreach (var connection in await ConnectionReader.ReadByUserAsync(user.Id))
                    await PortfolioSynchronizer.SyncAsync(connection);

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

    public class DashboardViewModel
    {
        public User User { get; set; }
        public Report Report { get; set; }
    }
}
