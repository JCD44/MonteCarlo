using MonteCarlo.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo.Data
{
    public class Portfolio : PortfolioValues
    {
        public Narvalo.Money InvestmentAmount { get; set; } = 0m.AsMoney();
        public Dictionary<string, decimal> TemporaryInvestmentAdjustments { get; set; } = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> ComputedInvestments()
        {
            Dictionary<string, decimal> values = new Dictionary<string, decimal>();
            foreach(var item in Investments)
            {
                values.Add(item.Key, item.Value);
            }
            
            foreach (var item in TemporaryInvestmentAdjustments)
            {
                var newValue = values[item.Key] + item.Value;
                values[item.Key] = newValue;
            }

            int attempts = 0;
            //Do we have over 100% or any value over 100% or under 0% (Currently no shorting/long strategies allowed)
            while (values.Sum(a => a.Value) > 100m || values.Any(a => a.Value > 100 || a.Value < 0))
            {
                var sum = values.Sum(a => a.Value);
                var hasBigOrSmallVal = values.Any(a => a.Value > 100 || a.Value < 0);
                if (sum > 100m || hasBigOrSmallVal)
                {
                    var offset = (sum - 100) / values.Count;
                    var values2 = new Dictionary<string, decimal>();
                    foreach (var item in values)
                    {
                        var newValue = item.Value - offset;
                        if (item.Value > 100)
                        {
                            newValue = 100;
                            
                            if (attempts > 10) newValue -= offset;//our other attempts didn't work, let's go the other way.
                        }
                        if (item.Value < 0)
                        {
                            newValue = 0;
                        }

                        values2.Upsert(item.Key, newValue);
                    }

                    values = values2;
                }

                if (attempts++ > 100)
                {
                    //If we are just a bit off, let's go with it.  If not, blow up.
                    var maxSafeVal = 102;
                    sum = values.Sum(a => a.Value);
                    if (sum > maxSafeVal) throw new System.Exception($"Portfolio values: {values.DictToString()} add up to {sum} > {maxSafeVal}.  While it should be 100, floating point numbers can do funny things, so we allow a little slack.  This goes beyond the safety limit so all calculations are suspect and are halted now.");

                    break;
                }
            }

            return values;   
        }

    }
}
