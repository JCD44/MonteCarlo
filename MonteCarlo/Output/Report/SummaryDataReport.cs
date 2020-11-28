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
        public void Report(List<OutcomeSummary> list, InputData input)
        {
            var percentage = input.PercentageOfInitialPortolioIsClose * 100;
            WriteManager.Write($" * Simulations: {list.Count}");
            WriteManager.Write($" * Failures: {list.Count(a => a.IsFail)}");
            WriteManager.Write($" * Close to Failures ({percentage}% or less initial portfolio): {list.Count(a => a.IsClose && !a.IsFail)}");
            WriteManager.Write($" * Failure Probability: {((decimal)((decimal)list.Count(a => a.IsFail) / (decimal)list.Count) * 100M).MakeReadable()}%");
            WriteManager.Write($" * Percentile Outcomes:");

            var outcomeInvestmentAmount = new List<Narvalo.Money>();
            foreach(var outcome in list)
            {
                outcomeInvestmentAmount.Add(outcome.Outcomes.Last().NewPortfolio.InvestmentAmount);
            }
            outcomeInvestmentAmount.Sort();
            var outcomesAsDecimal = new List<decimal>();

            foreach(var outcome in outcomeInvestmentAmount)
            {
                outcomesAsDecimal.Add(outcome.Amount);
            }

            WriteManager.Write($"   - Min: {outcomeInvestmentAmount.First().ToDecimal().MakeReadable()}");

            foreach (var item in new double[] { .001, .01, .03, .05, .08, .10, .25, .40, .50, .60, .75, .85, .95, .99, .999  }) {
                WriteManager.Write($"   - {item*100}%'tile: {outcomesAsDecimal.ToPercentile(item).MakeReadable()}");
            }
            WriteManager.Write($"   - Max: {outcomeInvestmentAmount.Last().ToDecimal().MakeReadable()}");


        }
    }
}
