using Microsoft.AspNetCore.Mvc;
using PortfolioInsight.Security;
using PortfolioInsight.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace PortfolioInsight.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        public AccountController(
            IIdentityReader identityReader,
            ISignInClient signInClient,
            ISignOutClient signOutClient)
        {
            IdentityReader = identityReader;
            SignInClient = signInClient;
            SignOutClient = signOutClient;
        }

        public IIdentityReader IdentityReader { get; }
        public ISignInClient SignInClient { get; }
        public ISignOutClient SignOutClient { get; }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await SignOutClient.SignOutAsync(HttpContext);
            return Redirect("/account/login");
        }
    }
}
