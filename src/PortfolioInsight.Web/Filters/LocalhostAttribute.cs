using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace PortfolioInsight.Web.Filters
{
    public class LocalhostAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Localhost: https://stackoverflow.com/a/66092244/188740
            // Filter: https://stackoverflow.com/a/59683135/188740
            if (context.HttpContext.Request.Host.Host != "localhost")
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
