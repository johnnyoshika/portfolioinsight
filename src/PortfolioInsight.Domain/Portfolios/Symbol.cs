using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class Symbol
    {
        public Symbol(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
