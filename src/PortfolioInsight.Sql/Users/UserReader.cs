using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Users
{
    [Service]
    public class UserReader : IUserReader
    {
        public UserReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<User> ReadByIdAsync(int id)
        {
            using (var context = Context())
                return await context
                    .Users
                    .Where(u => u.Id == id)
                    .Select(u => u.ToDto())
                    .FirstOrDefaultAsync();
        }
    }
}
