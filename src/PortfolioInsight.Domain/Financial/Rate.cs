using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PortfolioInsight.Financial.Converters;

namespace PortfolioInsight.Financial
{
    [TypeConverter(typeof(RateTypeConverter))]
    [JsonConverter(typeof(RateJsonConverter))]
    public struct Rate : IComparable<Rate>, IEquatable<Rate>
    {
        public static readonly Rate Zero = new Rate(0);
        public static readonly Rate Full = new Rate(1);

        public static Rate Max(Rate x, Rate y) => x > y ? x : y;
        public static Rate Min(Rate x, Rate y) => x < y ? x : y;

        public static implicit operator decimal(Rate rate) => rate.Value;
        public static explicit operator Rate(decimal value) => new Rate(value);

        public static bool operator ==(Rate left, Rate right) => left.Value == right.Value;
        public static bool operator !=(Rate left, Rate right) => left.Value != right.Value;

        public static bool operator >=(Rate left, Rate right) => left.Value >= right.Value;
        public static bool operator <=(Rate left, Rate right) => left.Value <= right.Value;

        public static bool operator >(Rate left, Rate right) => left.Value > right.Value;
        public static bool operator <(Rate left, Rate right) => left.Value < right.Value;

        public static Rate operator +(Rate left, Rate right) => new Rate(left.Value + right.Value);
        public static Rate operator -(Rate left, Rate right) => new Rate(left.Value - right.Value);
        public static Rate operator *(Rate left, Rate right) => new Rate(left.Value * right.Value);
        public static Rate operator /(Rate left, Rate right) => new Rate(left.Value / right.Value);

        public Rate(decimal value)
            : this()
        {
            Value = value;
        }

        public decimal Value { get; }
        public decimal Percent => Value * 100;
        public int Rounded => (int)Math.Round(Percent, 0, MidpointRounding.AwayFromZero);
        public override string ToString() => $"{Percent}%";
        public string RoundedTo(int decimals) => $"{Math.Round(Percent, decimals, MidpointRounding.AwayFromZero)}%";

        public int CompareTo(Rate other) =>
            Value.CompareTo(other.Value);

        public bool Equals(Rate other) =>
            Value.Equals(other.Value);

        public override bool Equals(object obj) =>
            obj is Rate && Equals((Rate)obj);

        public override int GetHashCode() =>
            Value.GetHashCode();
    }
}
