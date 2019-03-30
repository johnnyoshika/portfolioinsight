using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Authorizations
{
    public interface IAuthorizationWriter
    {
        Task WriteAsync(Authorization authorization);
    }
}
