using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioInsight.Authorizations;
using PortfolioInsight.Configuration;
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
            ITokenizer tokenizer)
        {
            UserReader = userReader;
            IdentityReader = identityReader;
            SignInClient = signInClient;
            SignOutClient = signOutClient;
            AuthenticationClient = authenticationClient;
            QuestradeSettings = questradeSettings;
            Tokenizer = tokenizer;
        }

        IUserReader UserReader { get; }
        IIdentityReader IdentityReader { get; }
        ISignInClient SignInClient { get; }
        ISignOutClient SignOutClient { get; }
        IAuthenticationClient AuthenticationClient { get; }
        IQuestradeSettings QuestradeSettings { get; }
        ITokenizer Tokenizer { get; }

        [Authorize]
        public async Task<IActionResult> Index() =>
            View(await AuthenticationClient.AuthenticateAsync(HttpContext.Request));

        [Authorize]
        [HttpGet("questrade/request")]
        public IActionResult QuestradeRequest() =>
            Redirect($"https://login.questrade.com/oauth2/authorize?client_id={QuestradeSettings.ConsumerKey}&response_type=code&redirect_uri={Request.AbsoluteUrl("/questrade/response").UrlEncode()}");

        [Authorize]
        [HttpGet("questrade/response")]
        public async Task<IActionResult> QuestradeResponse(string code)
        {
            await Tokenizer.ExchangeAsync(
                code,
                await AuthenticationClient.AuthenticateAsync(HttpContext.Request),
                Request.AbsoluteUrl("/questrade/response").UrlEncode());

            return Redirect("/");
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
    }
}
