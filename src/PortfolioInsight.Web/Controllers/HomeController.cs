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
            IPortfolioReader portfolioReader,
            IReporter reporter)
        {
            AuthenticationClient = authenticationClient;
            QuestradeSettings = questradeSettings;
            Tokenizer = tokenizer;
            ConnectionReader = connectionReader;
            AccountReader = accountReader;
            ConnectionSynchronizer = connectionSynchronizer;
            CurrencySynchronizer = currencySynchronizer;
            PortfolioReader = portfolioReader;
            Reporter = reporter;
        }

        IAuthenticationClient AuthenticationClient { get; }
        IQuestradeSettings QuestradeSettings { get; }
        ITokenizer Tokenizer { get; }
        IConnectionReader ConnectionReader { get; }
        IAccountReader AccountReader { get; }
        IConnectionSynchronizer ConnectionSynchronizer { get; }
        ICurrencySynchronizer CurrencySynchronizer { get; }
        IPortfolioReader PortfolioReader { get; }
        IReporter Reporter { get; }

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
        public async Task<IActionResult> Portfolio(int id)
        {
            var user = await AuthenticationClient.AuthenticateAsync(HttpContext.Request);
            if (!await PortfolioReader.UserOwnsPortfolio(id, user.Id))
                throw new UnauthorizedAccessException();

            return View(new PortfolioViewModel
            {
                User = user,
                Portfolio = await PortfolioReader.ReadByIdAsync(id),
                Report = await Reporter.GenerateAsync(user.Id, id)
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
