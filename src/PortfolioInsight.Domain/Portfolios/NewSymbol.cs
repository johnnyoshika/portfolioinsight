using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioInsight.Portfolios
{
    public record NewSymbol(
        string Name,
        string Description,
        string ListingExchangeCode,
        string CurrencyCode,
        int BrokerageId,
        string ReferenceId);
}
