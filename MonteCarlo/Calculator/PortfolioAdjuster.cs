using MonteCarlo.Calculator;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data
{
    public class PortfolioAdjuster
    {
        public PortfolioWork UpdatePortfolio(PortfolioWork portfolio)
        {
            foreach(var item in portfolio.Adjustments)
            {
                item.Init(portfolio);
                portfolio = item.Adjust(portfolio);
            }

            return portfolio;
        }
    }
}
