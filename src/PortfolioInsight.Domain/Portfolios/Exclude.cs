using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioInsight.Portfolios
{
    public class Exclude
    {
        public static Exclude None = new Exclude(new List<ExcludeAccount>(), new List<ExcludeSymbol>());

        public Exclude(IEnumerable<ExcludeAccount> accounts, IEnumerable<ExcludeSymbol> symbols)
        {
            Accounts = accounts.ToList();
            Symbols = symbols.ToList();
        }

        public IReadOnlyList<ExcludeAccount> Accounts { get; }
        public IReadOnlyList<ExcludeSymbol> Symbols { get; }
    }

    public class ExcludeAccount
    {
        public ExcludeAccount(int id) => Id = id;
        public int Id { get; }
    }

    public class ExcludeSymbol
    {
        public ExcludeSymbol(int accountId, int symbolId)
        {
            AccountId = accountId;
            SymbolId = symbolId;
        }

        public int AccountId { get; }
        public int SymbolId { get; }
    }
}
