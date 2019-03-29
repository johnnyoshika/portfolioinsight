using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PortfolioInsight.Web.Http
{
    public interface ISignOutClient
    {
        Task SignOutAsync(HttpContext context);
    }
}
