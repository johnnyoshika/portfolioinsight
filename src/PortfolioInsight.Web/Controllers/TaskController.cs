using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PortfolioInsight.Connections;
using PortfolioInsight.Portfolios;
using PortfolioInsight.Reports;
using PortfolioInsight.Users;
using PortfolioInsight.Web.Filters;
using System;
using System.Threading.Tasks;

namespace PortfolioInsight.Web.Controllers
{
    public class TaskController : Controller
    {
        public TaskController(
            IUserReader userReader,
            IConnectionReader connectionReader,
            IConnectionSynchronizer connectionSynchronizer,
            IPortfolioReader portfolioReader,
            IReportWriter reportWriter,
            IReporter reporter)
        {
            UserReader = userReader;
            ConnectionReader = connectionReader;
            ConnectionSynchronizer = connectionSynchronizer;
            PortfolioReader = portfolioReader;
            ReportWriter = reportWriter;
            Reporter = reporter;
        }

        IUserReader UserReader { get; }
        IConnectionReader ConnectionReader { get; }
        IConnectionSynchronizer ConnectionSynchronizer { get; }
        IPortfolioReader PortfolioReader { get; }
        IReportWriter ReportWriter { get; }
        IReporter Reporter { get; }


        [Localhost]
        [HttpPut("tasks/generate-reports")]
        public async Task<IActionResult> GenerateReports()
        {
            try
            {
                foreach (var user in await UserReader.ReadAllAsync())
                {
                    foreach (var connection in await ConnectionReader.ReadByUserIdAsync(user.Id))
                        await ConnectionSynchronizer.SyncAsync(connection);

                    foreach (var portfolio in await PortfolioReader.ReadByUserIdAsync(user.Id))
                        await ReportWriter.WriteAsync(portfolio.Id, MarketDate(), await Reporter.GenerateAsync(user.Id, portfolio.Id));
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // https://stackoverflow.com/a/5997619/188740
        DateTime EasternTime()
        {
            var timeUtc = DateTime.UtcNow;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
        }

        /// <summary>
        /// Return market closing date for snapshot
        /// </summary>
        DateTime MarketDate()
        {
            // Stock market hours in Eastern time: 9:30 to 16:00

            var easternTime = EasternTime();
            var time = easternTime.TimeOfDay;
            // TotalHours gives us fractional hours while Hours only gives us whole hours
            if (time.TotalHours > 9.5)
                return easternTime.Date;
            else
                return easternTime.Date.AddDays(-1);
        }
    }
}
