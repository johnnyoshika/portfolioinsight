using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    [Service]
    public class AssetClassReader : IAssetClassReader
    {
        public AssetClassReader(Func<Context> context, ICurrencyReader currencyReader)
        {
            Context = context;
            CurrencyReader = currencyReader;
        }

        Func<Context> Context { get; }
        ICurrencyReader CurrencyReader { get; }

        public async Task<AssetClass> ReadByIdAsync(int id) =>
            await ReadByAsync(c => c.Id == id);

        public async Task<AssetClass> ReadByNameAsync(int portfolioId, string name) =>
            await ReadByAsync(c => c.PortfolioId == portfolioId && c.Name == name);

        public async Task<AssetClass> ReadCashByPortfolioIdAsync(int portfolioId)
        {
            using (var context = Context())
            {
                var assetClass = await ReadByAsync(c => c.PortfolioId == portfolioId && c.Name == Balance.Cash);
                
                if (assetClass == null)
                {
                    var eAssetClass = new AssetClassEntity
                    {
                        PortfolioId = portfolioId,
                        Name = Balance.Cash
                    };
                    context.AssetClasses.Add(eAssetClass);
                    assetClass = eAssetClass.ToModel();
                }

                return assetClass;
            }
        }

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
