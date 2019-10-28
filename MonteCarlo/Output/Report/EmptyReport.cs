using System;
using System.Collections.Generic;
using System.Text;
using MonteCarlo.Data;

namespace MonteCarlo.Output.Report
{
    public class EmptyReport : IReport
    {
        public void Report(List<OutcomeSummary> list, InputData input)
        {
        }
    }
}
