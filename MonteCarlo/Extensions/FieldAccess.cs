using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class FieldAccess
    {
        public static T Get<T>(this IReader reader, string value, int index = 0)
        {
            try
            {
                return reader.GetField<T>(value, index);
            } catch (Exception e)
            {
                return default;
            }
        }

        public static string Get(this IReader reader, string value, int index = 0)
        {
            return reader.Get<string>(value, index);
        }

    }
}
