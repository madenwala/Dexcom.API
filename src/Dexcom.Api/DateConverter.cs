using Dexcom.Api.Models;
using Newtonsoft.Json;
using System;

namespace Dexcom.Api
{
    public class DateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
            //if (reader.TokenType == JsonToken.StartArray)
            //{
            //    return serializer.Deserialize<MyObject[]>(reader);
            //}
            //else
            //{
            //    var myObject = serializer.Deserialize<MyObject>(reader);
            //    return new MyObject[] { myObject };
            //}
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}