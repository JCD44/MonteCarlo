using CloneExtensions;
using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class AdjustByMaxMinValues : IPortfolioAdjustments
    {
        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            if (callCount++ > 20) return portfolio;
            var reprocess = false;
            if (max != null)
            {
                foreach (var item in max)
                {
                    var currentVal = portfolio.Portfolio.Investments[item.Key];
                    var maxVal = item.Value;
                    if (currentVal > maxVal)
                    {
                        var overage = currentVal - maxVal;
                        reprocess = true;
                        portfolio.Portfolio.Investments[item.Key] = maxVal;
                        var portList = portfolio.Portfolio.Investments.GetClone().Where(a => a.Key != item.Key).ToList();

                        //Warning: This could end up with rounding errors in the 10th decimal place--probably doesn't matter.
                        foreach (var otherInvestments in portList)
                        {
                            portfolio.Portfolio.Investments[otherInvestments.Key] = otherInvestments.Value + (decimal)(overage / portList.Count());
                        }
                    }
                }
            }

            //TODO: This can be defeated by Temporary changes like CAPE adjustments.
            if (min != null)
            {
                foreach (var item in min)
                {
                    var currentVal = portfolio.Portfolio.Investments[item.Key];
                    var minVal = item.Value;
                    if (currentVal < minVal)
                    {
                        var underage = minVal - currentVal;
                        reprocess = true;
                        portfolio.Portfolio.Investments[item.Key] = minVal;
                        var portList = portfolio.Portfolio.Investments.GetClone().Where(a => a.Key != item.Key).ToList();

                        var additionalOffset = 0M;
                        do
                        {
                            var newOffset = additionalOffset;
                            additionalOffset = 0M;
                            //Warning: This could end up with rounding errors in the 10th decimal place--probably doesn't matter.
                            foreach (var otherInvestments in portList)
                            {
                                var valueAdjustment = otherInvestments.Value - (decimal)((underage + newOffset) / portList.Count());
                                if (min.ContainsKey(otherInvestments.Key))
                                {
                                    if (min[otherInvestments.Key] > valueAdjustment)
                                    {
                                        additionalOffset += min[otherInvestments.Key] - valueAdjustment;
                                        valueAdjustment = min[otherInvestments.Key];
                                    }
                                }
                                portfolio.Portfolio.Investments[otherInvestments.Key] = valueAdjustment;
                            }
                            if (additionalOffset != 0)
                            {
                                portList = portList.Where(a => portfolio.Portfolio.Investments[a.Key] != min.GetValueOrDefault(a.Key)).ToList();
                            }
                            underage = 0;
                        } while (additionalOffset != 0);
                    }
                }
            }

            if (reprocess) return Adjust(portfolio);

            return portfolio;
        }

        private int callCount = 0;
        private readonly Dictionary<string, decimal> min;
        private readonly Dictionary<string, decimal> max;

        public AdjustByMaxMinValues(Dictionary<string, decimal> min, Dictionary<string, decimal> max)
        {
            this.min = min;
            this.max = max;
        }

        public void Init(PortfolioWork portfolio)
        {
            callCount = 0;
        }
    }
}
