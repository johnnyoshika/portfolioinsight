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
            IAuthenticationClient authenticationClient,
            IPortfolioReader portfolioReader,
            IConnectionReader connectionReader,
            ISymbolReader symbolReader,
            IAllocationWriter allocationWriter,
            IAssetClassWriter assetClassWriter)
        {
            AuthenticationClient = authenticationClient;
            PortfolioReader = portfolioReader;
            ConnectionReader = connectionReader;
            SymbolReader = symbolReader;
            AllocationWriter = allocationWriter;
            AssetClassWriter = assetClassWriter;
        }

        IAuthenticationClient AuthenticationClient { get; }
        IPortfolioReader PortfolioReader { get; }
        IConnectionReader ConnectionReader { get; }
        ISymbolReader SymbolReader { get; }
        IAllocationWriter AllocationWriter { get; }
        IAssetClassWriter AssetClassWriter { get; }

        [Localhost]
        [HttpPut]
        public async Task<IActionResult> Index()
        {
            await Portfolio1();
            await Portfolio5();
            return NoContent();
        }

        async Task Portfolio1()
        {
            int portfolioId = 1;

            var allocations = new Allocation[]
            {
                new Allocation(await SymbolReader.ReadByNameAsync("XEF.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "INTL", (Rate)0.16m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XEC.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "EM", (Rate)0.065m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XIC.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CA", (Rate)0.225m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XUU.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("VUN.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ITOT"  ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.25m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ZDB.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.30m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ZAG.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.30m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.U.TO"), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)})
            };

            foreach (var allocation in allocations)
                await AllocationWriter.WriteAsync(portfolioId, allocation);
        }

        async Task Portfolio5()
        {
            int portfolioId = 5;
            var connection = await ConnectionReader.ReadByIdAsync(1); // Using 'connection' b/c VUN.TO, AVUV, and AVDV weren't in my portfolio when I first added these allocations

            var allocations = new Allocation[]
            {
                new Allocation(await SymbolReader.ReadByNameAsync("ZDB.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.275m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ZAG.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "BOND", (Rate)0.275m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XIC.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CA", (Rate)0.217m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XUU.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.21m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("VUN.TO",   connection), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.21m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("ITOT"                ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US", (Rate)0.21m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("AVUV",     connection), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "US SCV", (Rate)0.08m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XEF.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "INTL", (Rate)0.112m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("AVDV",     connection), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "INTL SCV", (Rate)0.05m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("XEC.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "EM", (Rate)0.056m), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.TO"              ), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.U.TO", connection), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)}),
                new Allocation(await SymbolReader.ReadByNameAsync("DLR.U.TO", connection), new[]{ new AllocationProportion(await AssetClassWriter.WriteAsync(portfolioId, "CASH", Rate.Zero), Rate.Full)})
            };

            foreach (var allocation in allocations)
                await AllocationWriter.WriteAsync(portfolioId, allocation);
        }
    }
}
