using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class ReadableNumber
    {
        public static string MakeReadable(this decimal value)
        {
            return String.Format("{0:n}", Math.Round(value, 2));
        }
        public static string MakeReadable(this Narvalo.Money money)
        {
            return money.ToDecimal().MakeReadable();
        }
    }
}
