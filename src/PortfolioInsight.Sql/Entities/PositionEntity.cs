using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class PositionEntity
    {
        public Position ToModel() =>
            new Position(
                Symbol.ToModel(),
                Value);

        public PositionEntity Assign(Position position)
        {
            Value = position.Value;
            SymbolId = position.Symbol.Id;
            return this;
        }
    }
}
