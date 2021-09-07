using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class ApplicationUrl
    {
        public static string AbsoluteUrl(this HttpRequest request, string path) =>
            request.AbsoluteHost() + path;

        public static string AbsoluteHost(this HttpRequest request) =>
            "https://app.portfolioinsight.local:44348";
    }
}
