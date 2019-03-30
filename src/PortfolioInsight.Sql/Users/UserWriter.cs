using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Users
{
    [Service]
    public class UserWriter : IUserWriter
    {
        public UserWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task WriteAsync(User user)
        {
            using (var context = Context())
            {
                var eUser = await context
                    .Users
                    .Where(u => u.Id == user.Id)
                    .FirstOrDefaultAsync();

                if (eUser == null)
                {
                    eUser = new UserEntity();
                    context.Users.Add(eUser);
                }

                eUser.Assign(user);
                await context.SaveChangesAsync();
                user.Id = eUser.Id;
            }
        }
    }
}
