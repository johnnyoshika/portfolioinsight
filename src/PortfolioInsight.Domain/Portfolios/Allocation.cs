using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class Allocation
    {

        public Allocation(Symbol symbol)
            : this(symbol, new List<AllocationProportion>())
        {
        }

        public Allocation(Symbol symbol, IEnumerable<AllocationProportion> proportions)
        {
            Symbol = symbol;
            Proportions = proportions.ToList();
        }

        public Symbol Symbol { get; }

        IReadOnlyList<AllocationProportion> _proportions;
        public IReadOnlyList<AllocationProportion> Proportions
        {
            get => _proportions;
            set
            {
                if (value.Sum(p => p.Rate) != Rate.Full)
                    throw new ArgumentException("Proportions must add to full rate.");

                _proportions = value;
            }
        }
    }
}
