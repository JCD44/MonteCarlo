using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo.Calculator
{
    internal static class AdjustHelper
    {
        internal static bool ShouldExecute(IPortfolioAdjustments adjustment, PortfolioWork portfolio)
        {
            if (adjustment.StartMonth > portfolio.MonthProcessed) return false;
            if (adjustment.EndMonth != null && adjustment.EndMonth < portfolio.MonthProcessed) return false;

            return true;
        }
    }
}
