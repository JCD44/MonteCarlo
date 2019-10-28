using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data
{
    public class MonthlyReturn
    {
        public Narvalo.Money MonthProfitOrLoss { get; set; }
        public Dictionary<string, Narvalo.Money> Returns { get; private set; } = new Dictionary<string, Narvalo.Money>();
    }
}
