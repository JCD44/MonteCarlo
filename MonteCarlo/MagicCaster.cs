using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo
{
    public static class MagicCaster
    {
        public static decimal? ToNumber(this string value)
        {
            var newVal = value;
            bool isPercent = false;
            if (string.IsNullOrEmpty(value)) return null;
            if (value.EndsWith("%"))
            {
                newVal = value.Substring(0, value.Length - 1);
                isPercent = true;
            }
            if (value.Contains(",")) newVal = newVal.Replace(",", "");
            decimal result;
            if (decimal.TryParse(newVal, out result))
            {
                if (isPercent) result *= .01M;
                return result;
            }
            return null;

        }
    }
}
