using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PortfolioInsight
{
    public abstract class Entity<T> where T : Entity<T>
    {
        internal abstract IEnumerable<object> EqualityAttributes { get; }

        public void Assign(T source)
        {
            foreach (var property in typeof(T).GetTypeInfo().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty |
                BindingFlags.SetProperty))
            {
                // only assign simple types (e.g. int, string) and not navigational properties
                if (!property.PropertyType.GetTypeInfo().IsValueType &&
                    property.PropertyType != typeof(string))
                    continue;

                var value = property.GetValue(source);
                property.SetValue(this, value);
            }
        }
    }

    public class IdComparer<T> : IEqualityComparer<T> where T : Entity<T>
    {
        public bool Equals(T x, T y) =>
            x != null && y != null && x.EqualityAttributes
                .SequenceEqual(y.EqualityAttributes);

        public int GetHashCode(T obj) =>
            obj.EqualityAttributes.Aggregate(0, (h, a) => h ^ a.GetHashCode());
    }
}
