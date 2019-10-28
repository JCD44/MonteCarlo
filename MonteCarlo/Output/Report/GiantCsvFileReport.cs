using System;
using System.Collections.Generic;
using System.Text;
using MonteCarlo.Data;

namespace MonteCarlo.Output.Report
{
    public class GiantCsvFileReport : GenericCsvReport
    {
        public override void Report(List<OutcomeSummary> list, InputData input)
        {
            var filename = $"Execution_{Guid.NewGuid()}.csv";
            var path = System.IO.Path.GetTempPath();
            var fullPath = path + filename;

            var text = CreateGiantCsv(list, input);

            WriteManager.Write($"Writing data to {fullPath}");
            System.IO.File.WriteAllText(fullPath, text);
        }
    }
}
