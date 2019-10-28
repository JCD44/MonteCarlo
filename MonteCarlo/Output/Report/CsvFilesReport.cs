using System.Collections.Generic;
using MonteCarlo.Data;

namespace MonteCarlo.Output.Report
{
    public class CsvFilesReport : GenericCsvReport
    {
        public override void Report(List<OutcomeSummary> list, InputData input)
        {
            foreach (var item in CreateCsvReport(list, input))
            {
                WriteManager.Write($"Writing data to {item.Key}");
                System.IO.File.WriteAllText(item.Key, item.Value);
            }
        }
    }
}
