using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class AssetClassReader : IAssetClassReader
    {
        public AssetClassReader(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<AssetClass> ReadByIdAsync(int id) =>
            await ReadByAsync(c => c.Id == id);

        public async Task<AssetClass> ReadByNameAsync(int userId, string name) =>
            await ReadByAsync(c => c.UserId == userId && c.Name == name);

        async Task<AssetClass> ReadByAsync(Expression<Func<AssetClassEntity, bool>> filter)
        {
            using (var context = Context())
                return await context
                    .AssetClasses
                    .Where(filter)
                    .Select(c => c.ToModel())
                    .FirstOrDefaultAsync();
        }
    }
}
