﻿using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Financial;

namespace PortfolioInsight.Portfolios
{
    public class AssetClass : ValueObject<AssetClass>
    {
        public static readonly AssetClass Unknown = new AssetClass(0, "???", null);

        public AssetClass(int id, string name, Rate? target)
        {
            Id = id;
            Name = name;
            Target = target;
        }

        public int Id { get; }
        public string Name { get; }
        public Rate? Target { get; }

        public override string ToString() => Name;

        protected override IEnumerable<object> EqualityCheckAttributes =>
            new object[] { Id, Name, Target };
    }
}
