using Narvalo;
using System.Collections.Generic;

namespace MonteCarlo.Data
{
    public class MonthlyReturn
    {
        public Money MonthProfitOrLoss { get; set; }
        public Dictionary<string, Money> Returns { get; private set; } = new Dictionary<string, Money>();
    }
}
