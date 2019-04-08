using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class AssetClassWriter : IAssetClassWriter
    {
        public AssetClassWriter(Func<Context> context)
        {
            Context = context;
        }

        Func<Context> Context { get; }

        public async Task<AssetClass> WriteAsync(int userId, string name, Rate? target)
        {
            using (var context = Context())
            {
                var eAssetClass = await context
                    .AssetClasses
                    .Where(c => c.UserId == userId && c.Name == name)
                    .FirstOrDefaultAsync();

                if (eAssetClass == null)
                {
                    eAssetClass = new AssetClassEntity
                    {
                        UserId = userId,
                        Name = name
                    };

                    context.AssetClasses.Add(eAssetClass);
                }

                eAssetClass.Target = target;
                await context.SaveChangesAsync();
                return eAssetClass.ToModel();
            }
        }
    }
}
