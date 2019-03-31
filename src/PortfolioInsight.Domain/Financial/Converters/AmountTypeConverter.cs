using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace PortfolioInsight.Financial.Converters
{
    public class AmountTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) ||
            base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) =>
            value is string
            ? (Amount)Convert.ToDecimal(value)
            : base.ConvertFrom(context, culture, value);
    }
}
