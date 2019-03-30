using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Users
{
    public interface IUserReader
    {
        Task<User> ReadByIdAsync(int id);
    }
}
