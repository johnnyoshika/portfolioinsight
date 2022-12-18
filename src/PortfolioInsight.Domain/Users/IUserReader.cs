using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Users
{
    public interface IUserReader
    {
        Task<List<User>> ReadAllAsync();
        Task<User> ReadByIdAsync(int id);
    }
}
