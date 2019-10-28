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
        }
    }
}
