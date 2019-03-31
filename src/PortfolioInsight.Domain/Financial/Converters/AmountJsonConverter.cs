using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PortfolioInsight.Financial.Converters
{
    public class AmountJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Amount);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            (Amount)Convert.ToDecimal(reader.Value);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            writer.WriteValue((decimal)(Amount)value);
    }
}
