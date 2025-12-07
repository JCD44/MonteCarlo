using MonteCarlo.Data;
using System.Collections.Generic;

namespace MonteCarlo.Output.Report
{
    public interface IReport
    {
        void Report(List<OutcomeSummary> list, InputData input);
    }
}
