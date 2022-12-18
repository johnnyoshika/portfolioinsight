using Newtonsoft.Json;
using PortfolioInsight.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight
{
    public partial class ReportEntity
    {
        public Report ToModel() =>
            JsonConvert.DeserializeObject<Report>(Json);

        public ReportEntity Assign(DateTime createdAt, Report report)
        {
            CreatedAt = createdAt;
            Json = JsonConvert.SerializeObject(report);
            return this;
        }
    }
}
