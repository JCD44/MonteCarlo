using MonteCarlo.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data.JSON
{
    public class DeserializeMoney : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Narvalo.Money) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {                
                serializer.Serialize(writer, value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = 0m.AsMoney();


            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        //propertyName = Convert.ToString(reader.Value);

                        break;
                    case JsonToken.EndObject:
                        return result;
                    default:
                        result = reader.Value.AsMoneyByCast();
                        break;
                }
            }

            return result;
        }
    }

}
