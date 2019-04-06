using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight
{
    static class DbSetSync
    {
        public static void Sync<T>(this DbSet<T> set, ICollection<T> entities, IEnumerable<T> source)
             where T : Entity<T>
        {
            set.RemoveIf(entities, e => !source.Contains(e, new IdComparer<T>()));
            source.CopyTo(entities);
        }

        static void RemoveIf<T>(this DbSet<T> set, ICollection<T> entities, Predicate<T> condition)
             where T : Entity<T>
        {
            foreach (var e in entities)
                if (condition(e))
                    set.Remove(e);
        }

        static void CopyTo<T>(this IEnumerable<T> source, ICollection<T> entities)
             where T : Entity<T>
        {
            var original = entities.ToList();
            var comparer = new IdComparer<T>();
            foreach (var s in source)
                if (original.Contains(s, comparer))
                    entities.First(e => comparer.Equals(e, s)).Assign(s);
                else
                    entities.Add(s);
        }
    }
}
