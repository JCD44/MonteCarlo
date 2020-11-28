using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonteCarlo.Data;
using MonteCarlo.Extensions;

namespace MonteCarlo.Output.Report
{
    public class SummaryDataReport : IReport
    {
        private void Percentiles(List<OutcomeSummary> list, InputData input)
        {
            WriteManager.Write($" * Percentile Outcomes:");

            var outcomeInvestmentAmount = new List<Narvalo.Money>();
            foreach (var outcome in list)
            {
                outcomeInvestmentAmount.Add(outcome.Outcomes.Last().NewPortfolio.InvestmentAmount);
            }
            outcomeInvestmentAmount.Sort();
            var outcomesAsDecimal = new List<decimal>();

            foreach (var outcome in outcomeInvestmentAmount)
            {
                outcomesAsDecimal.Add(outcome.Amount);
            }

            WriteManager.Write($"   - Min: {outcomeInvestmentAmount.First().ToDecimal().MakeReadable()}");

            foreach (var item in new double[] { .001, .01, .03, .05, .08, .10, .25, .40, .50, .60, .75, .85, .95, .99, .999 })
            {
                var value = $"000{item * 100}";
                WriteManager.Write($"   - {value.Substring(value.Length-4)}%'tile: {outcomesAsDecimal.ToPercentile(item).MakeReadable()}");
            }
            WriteManager.Write($"   - Max: {outcomeInvestmentAmount.Last().ToDecimal().MakeReadable()}");
        }

        private int PercentageDropCalculation(List<decimal> worstAmounts, decimal low, decimal high)
        {
            return worstAmounts.Count(a => a < high && a >= low);
        }


        private void PercentageDropFromInitialInvestment(List<OutcomeSummary> list, InputData input)
        {
            WriteManager.Write($" * % of portfolios that fell between:");
            var initAmt = input.InitialAmount;

            var tenPercentOfInitAmt = initAmt / 10;

            var outcomeInvestmentAmount = new List<decimal>();
            //var lowestValue = initAmt;
            foreach (var outcomeSummary in list)
            {
  
                //foreach (var outcome in outcomeSummary.Outcomes)
                //{
                //    var checkedValue = outcome.NewPortfolio.InvestmentAmount;
                //    if (lowestValue>checkedValue)
                //    {
                //        lowestValue = checkedValue;
                //    }
                    
                //}
                outcomeInvestmentAmount.Add(outcomeSummary.WorstAmount);
                //lowestValue = initAmt;
            }

            WriteManager.Write($"  - Portfolios that fell between -infinity-10%: {PercentageDropCalculation(outcomeInvestmentAmount, decimal.MinValue, tenPercentOfInitAmt)}");

            foreach (var item in new decimal[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 })
            {
                var dec = .1M * item;

                WriteManager.Write($"  - Portfolios that fell between {item}-{item+10}%: {PercentageDropCalculation(outcomeInvestmentAmount, dec * tenPercentOfInitAmt, (dec+1) * tenPercentOfInitAmt)}");
            }


        }

        public void Report(List<OutcomeSummary> list, InputData input)
        {
            var percentage = input.PercentageOfInitialPortolioIsClose * 100;
            WriteManager.Write($" * Simulations: {list.Count}");
            WriteManager.Write($" * Failures: {list.Count(a => a.IsFail)}");
            WriteManager.Write($" * Close to Failures, but not failed ({percentage}% or less initial portfolio): {list.Count(a => a.IsClose && !a.IsFail)}");
            WriteManager.Write($" * Failure Probability: {((decimal)((decimal)list.Count(a => a.IsFail) / (decimal)list.Count) * 100M).MakeReadable()}%");

            Percentiles(list, input);

            PercentageDropFromInitialInvestment(list, input);

        }
    }
}
