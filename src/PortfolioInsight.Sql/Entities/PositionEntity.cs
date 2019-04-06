using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Portfolios;

namespace PortfolioInsight
{
    public partial class PositionEntity : Entity<PositionEntity>
    {
        internal override IEnumerable<object> EqualityAttributes =>
            new object[] { Id };

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
