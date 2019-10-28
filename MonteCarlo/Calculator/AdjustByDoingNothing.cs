using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class AdjustByDoingNothing : IPortfolioAdjustments
    {
        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            return portfolio;
        }

        public void Init(PortfolioWork portfolio)
        {
        }
    }
}
