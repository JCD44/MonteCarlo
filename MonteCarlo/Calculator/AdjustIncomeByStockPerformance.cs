using CloneExtensions;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    /// <summary>
    /// Literally this is what drives the "Performance".  This should be the dead last thing in the list to run.
    /// </summary>
    public class AdjustIncomeByStockPerformance : IPortfolioAdjustments
    {
        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            var port = portfolio.Portfolio;
            var ret = portfolio.ReturnData;
            var pl = 0M.AsMoney();
            foreach(var invest in port.ComputedInvestments())
            {
                var investmentPercentage = invest.Value;
                if (investmentPercentage > 1) investmentPercentage *= .01M;
                var weightedReturn = (decimal) ret.NameToPercentageReturn[invest.Key] * investmentPercentage;
                var amt = port.InvestmentAmount.GetClone();

                //Perhaps the best way to handle this is to note if you have 0 or less invested, then the performance ultimately should be 0.
                if (port.InvestmentAmount<=0) amt = 0M.AsMoney();
                var investmentReturn = weightedReturn * amt;
                pl += investmentReturn;
                portfolio.Returns.Returns.Add(invest.Key, investmentReturn);
            }
            portfolio.Returns.MonthProfitOrLoss = pl;

            port.InvestmentAmount += pl;

            return portfolio;
        }
        public void Init(PortfolioWork portfolio)
        {
        }
    }
}
