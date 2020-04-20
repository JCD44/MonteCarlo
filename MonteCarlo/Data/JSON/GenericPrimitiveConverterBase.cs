using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data.JSON
{
    public class GenericPrimitiveConverterBase<T> : JsonConverter
    {
        protected virtual object GetValue(T value) { return value; }

        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null || value == DBNull.Value)
            {
                writer.WriteNull();
            }
            else
            {
                var nullableType = Nullable.GetUnderlyingType(typeof(T));
                if (nullableType != null)
                {
                    var convertedValue = Convert.ChangeType(value, nullableType);
                    serializer.Serialize(writer, convertedValue);
                }
                else
                {
                    T tValue = (T)value;
                    serializer.Serialize(writer, GetValue(tValue));
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return default(T);
            return serializer.Deserialize<T>(reader);
        }
    }
}