using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class AdjustByDoingNothing : IPortfolioAdjustments
    {
        public int StartMonth { get; set; }
        public int? EndMonth { get; set; }
        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            return portfolio;
        }

        public void Init(PortfolioWork portfolio)
        {
            StartMonth = 0; 
            EndMonth = null;
        }
    }
}
