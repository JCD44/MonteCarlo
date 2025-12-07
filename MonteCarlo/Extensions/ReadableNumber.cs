using System;
using NodaMoney;

namespace MonteCarlo.Extensions
{
    public static class ReadableNumber
    {
        public static string MakeReadable(this decimal value)
        {
            return String.Format("{0:n}", Math.Round(value, 2));
        }
        public static string MakeReadable(this Money money)
        {
            return money.ToDecimal().MakeReadable();
        }
    }
}
