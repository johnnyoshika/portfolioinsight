using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using PortfolioInsight.Financial.Converters;

namespace PortfolioInsight.Financial
{
    [TypeConverter(typeof(AmountTypeConverter))]
    [JsonConverter(typeof(AmountJsonConverter))]
    public struct Amount : IComparable<Amount>, IEquatable<Amount>
    {
        public static readonly Amount Zero = new Amount(0);

        public static implicit operator decimal(Amount amount) => amount.Value;
        public static implicit operator Amount(decimal value) => new Amount(value);

        public static bool operator ==(Amount left, Amount right) => left.Value == right.Value;
        public static bool operator !=(Amount left, Amount right) => left.Value != right.Value;

        public static bool operator >=(Amount left, Amount right) => left.Value >= right.Value;
        public static bool operator <=(Amount left, Amount right) => left.Value <= right.Value;

        public static bool operator >(Amount left, Amount right) => left.Value > right.Value;
        public static bool operator <(Amount left, Amount right) => left.Value < right.Value;

        public static Amount operator +(Amount left, Amount right) => new Amount(left.Value + right.Value);
        public static Amount operator -(Amount left, Amount right) => new Amount(left.Value - right.Value);
        public static Amount operator *(Amount left, Amount right) => new Amount(left.Value * right.Value);
        public static Amount operator /(Amount left, Amount right) => new Amount(left.Value / right.Value);

        public static Amount operator *(Amount left, Rate right) => new Amount(left.Value * right.Value);
        public static Amount operator *(Rate left, Amount right) => new Amount(left.Value * right.Value);

        public Amount(decimal value)
            : this()
        {
            Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public decimal Value { get; }
        public override string ToString() => $"${Value:N2}";

        public int CompareTo(Amount other) =>
            Value.CompareTo(other.Value);

        public bool Equals(Amount other) =>
            Value.Equals(other.Value);

        public override bool Equals(object obj) =>
            obj is Amount && Equals((Amount)obj);

        public override int GetHashCode() =>
            Value.GetHashCode();
    }
}
