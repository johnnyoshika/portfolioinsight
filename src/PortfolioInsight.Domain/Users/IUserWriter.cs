using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Users
{
    public interface IUserWriter
    {
        Task WriteAsync(User user);
    }
}
