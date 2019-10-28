using System.Collections.Generic;
using MonteCarlo.Data;
using MonteCarlo.Extensions;

namespace MonteCarlo.Output.Report
{
    public class VerboseReport : IReport
    {
        public void Report(List<OutcomeSummary> list, InputData input)
        {
            foreach (var summary in list)
            {
                WriteManager.Write($"***********Run # {summary.RowNumber}***********");
                WriteManager.Write("-----");
                WriteManager.Write($"Started: {summary.PortolioStartTime.PrettyDate()} ");
                WriteManager.Write($"- Is Failure: {summary.IsFail}");
                WriteManager.Write($"- Is Close: {summary.IsClose}");
                WriteManager.Write($"- Worst Month: {summary.WorstMonth}");
                WriteManager.Write($"- Worst Amount: {summary.WorstAmount.MakeReadable()}");
                WriteManager.Write($"- Total Returns By Category: {summary.TotalReturnsByCategory.DictToString()}");
                WriteManager.Write("-----");

                var period = 0;
                foreach (var item in summary.Outcomes)
                {
                    period++;
                    WriteManager.Write($"Period: {period} - {item}");
                }
                WriteManager.Write($"***********End of Run # {summary.RowNumber}***********");
            }
        }
    }
}
