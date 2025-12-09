using MonteCarlo.Data;
using Newtonsoft.Json.Bson;
using NodaMoney;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class DictionaryStringToDecimal
    {
        public static void Upsert(this Dictionary<string, decimal> dict, string key, decimal value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, 0);

            dict[key] += value;
        }

        public static Dictionary<string, decimal> CloneTo(this Dictionary<string, decimal> dictFrom, Dictionary<string, decimal> dictTo)
        {
            foreach (var from in dictFrom)
                dictTo.Add(from.Key, from.Value);

            return dictTo;
        }

        public static Dictionary<string, decimal> Clone(this Dictionary<string, decimal> dictFrom)
        {
            return dictFrom.CloneTo(new Dictionary<string, decimal>());
        }

        public static void Upsert(this Dictionary<string, decimal?> dict, string key, decimal? value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, 0);
            if (!value.HasValue)
                dict[key] = value;
            else 
               dict[key] += value;
        }

        public static string DictToString(this Dictionary<string, decimal> dict, string dividingChars = "; ", string quoteVal = "'")
        {
            var sb = new StringBuilder();
            foreach (var item in dict)
            {
                sb.Append(item.Key + "=" + quoteVal + item.Value + quoteVal + dividingChars);
            }
            return sb.ToString().Trim();
        }

        public static string DictToString(this Dictionary<string, decimal?> dict, string dividingChars = "; ", string quoteVal = "'")
        {
            var sb = new StringBuilder();
            foreach (var item in dict)
            {
                sb.Append(item.Key + "=" + quoteVal + item.Value + quoteVal + dividingChars);
            }
            return sb.ToString().Trim();
        }

        public static string DictToString(this Dictionary<string, Money> dict, string dividingChars = "; ", string quoteVal = "'")
        {
            var sb = new StringBuilder();
            foreach (var item in dict)
            {
                sb.Append(item.Key + "=" + quoteVal + item.Value.MakeReadable() + quoteVal + dividingChars);
            }
            return sb.ToString().Trim();
        }
    }
}
