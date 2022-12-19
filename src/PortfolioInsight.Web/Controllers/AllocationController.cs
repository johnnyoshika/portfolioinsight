using Microsoft.AspNetCore.Mvc;
using PortfolioInsight.Connections;
using PortfolioInsight.Financial;
using PortfolioInsight.Portfolios;
using PortfolioInsight.Web.Filters;
using PortfolioInsight.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioInsight.Web.Controllers
{
    [Route("create-allocations")]
    public class AllocationController : Controller
    {
        public AllocationController(
            ITokenizer tokenizer,
            IPortfolioReader portfolioReader,
            IConnectionReader connectionReader,
            ISymbolReader symbolReader,
            IAllocationWriter allocationWriter,
            IAssetClassWriter assetClassWriter)
        {
            Tokenizer = tokenizer;
            PortfolioReader = portfolioReader;
            ConnectionReader = connectionReader;
            SymbolReader = symbolReader;
            AllocationWriter = allocationWriter;
            AssetClassWriter = assetClassWriter;
        }

        ITokenizer Tokenizer { get; }
        IPortfolioReader PortfolioReader { get; }
        IConnectionReader ConnectionReader { get; }
        ISymbolReader SymbolReader { get; }
        IAllocationWriter AllocationWriter { get; }
        IAssetClassWriter AssetClassWriter { get; }

        [Localhost]
        [HttpPut]
        public async Task<IActionResult> Index()
        {
            var connection = await ConnectionReader.ReadByIdAsync(1);
            // Using 'accessToken' b/c we want SymbolReader.ReadByNameAsync to create a new symbol if one doesn't exist
            var accessToken = await Tokenizer.RefreshAsync(connection);
            await Portfolio1(accessToken);
            await Portfolio5(accessToken);
            return NoContent();
        }

        async Task Portfolio1(AccessToken accessToken)
        {
            int portfolioId = 1;

            var allocations = new Allocation[]
            {
                new Allocation(await SymbolReader.ReadByNameAsync("XEF.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "INTL", (Rate)0.16m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XEC.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "EM", (Rate)0.065m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XIC.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CA", (Rate)0.225m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XUU.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("VUN.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ITOT"    , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ZDB.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.30m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ZAG.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.30m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.U.TO", accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)})
            };

            foreach (var allocation in allocations)
                await AllocationWriter.WriteAsync(portfolioId, allocation);
        }

        async Task Portfolio5(AccessToken accessToken)
        {
            int portfolioId = 5;
            var connection = await ConnectionReader.ReadByIdAsync(1); // Using 'connection' b/c we want SymbolReader.ReadByNameAsync to create a new symbol if one doesn't exist

            var allocations = new Allocation[]
            {
                new Allocation(await SymbolReader.ReadByNameAsync("ZDB.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.275m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ZAG.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.275m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XIC.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CA", (Rate)0.217m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XUU.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.21m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("VUN.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.21m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ITOT"    , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.21m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("AVUV"    , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US SCV", (Rate)0.08m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XEF.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "INTL", (Rate)0.112m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("AVDV"    , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "INTL SCV", (Rate)0.05m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XEC.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "EM", (Rate)0.056m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.TO"  , accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.U.TO", accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.U.TO", accessToken), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)})
            };

            foreach (var allocation in allocations)
                await AllocationWriter.WriteAsync(portfolioId, allocation);
        }
    }
}
