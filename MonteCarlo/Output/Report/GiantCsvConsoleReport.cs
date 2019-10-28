using System.Collections.Generic;
using MonteCarlo.Data;

namespace MonteCarlo.Output.Report
{
    public class GiantCsvConsoleReport : GenericCsvReport
    {
        public IWriter WriterToResetTo { get; set; } = new ConsoleWriter();

        public override void Report(List<OutcomeSummary> list, InputData input)
        {
            //I'm imagining a time where an external program like a website call the exe and wants only this output...
            WriteManager.Writer = WriterToResetTo;
            var text = CreateGiantCsv(list, input);
            WriteManager.Write(text);
        }
    }
}
