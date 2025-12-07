using MonteCarlo.Extensions;
using Newtonsoft.Json;
using System;
using NodaMoney;

namespace MonteCarlo.Data.JSON
{
    public class DeserializeMoney : JsonConverter<Money>
    {
        public override void WriteJson(JsonWriter writer, Money value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.Amount);
            }
        }

        public override Money ReadJson(JsonReader reader, Type objectType, Money existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            {
                return reader.Value.AsMoneyByCast();
            }

            if (reader.TokenType == JsonToken.Null) return 0m.AsMoney();

            if (reader.TokenType == JsonToken.String)
            {
                return reader.Value.AsMoneyByCast();
            }

            throw new JsonSerializationException($"Invalid token type {reader.TokenType} for Money.");
        }
    }

}
