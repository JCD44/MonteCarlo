using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class DateTimeExtension
    {
        public static string PrettyDate(this DateTime date)
        {
            return date.Date.ToShortDateString();
        }
    }
}
