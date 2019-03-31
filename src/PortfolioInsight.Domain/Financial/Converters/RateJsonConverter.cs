using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PortfolioInsight.Financial.Converters
{
    public class RateJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Rate);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            (Rate)Convert.ToDecimal(reader.Value);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            writer.WriteValue((decimal)(Rate)value);
    }
}
