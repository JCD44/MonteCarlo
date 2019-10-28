using MonteCarlo.Data;
using MonteCarlo.Dispatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Simulations
{
    public interface ISimulation
    {
        OutputMode Mode { get; set; }
        List<OutcomeSummary> ExecuteSimulate(InputData input);
    }
}
