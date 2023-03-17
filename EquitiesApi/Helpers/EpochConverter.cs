/*
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EquitiesApi.Helpers
{
    public class EpochConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObject = new JObject();
            jObject["$date"] = new DateTimeOffset((DateTime)value).ToUnixTimeMilliseconds();
            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var epoch = jObject.GetValue("$date").Value<long>();
            return DateTimeOffset.FromUnixTimeMilliseconds(epoch).UtcDateTime;
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);
    }
}
*/