using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Output.Report
{
    public interface IReport
    {
        void Report(List<OutcomeSummary> list, InputData input);
    }
}
