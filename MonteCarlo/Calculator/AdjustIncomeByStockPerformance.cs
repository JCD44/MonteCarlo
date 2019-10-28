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
                //if (amt.IsNegative) amt = amt.Negate();
                var investmentReturn = weightedReturn * amt;//TODO Make absolute value -- negative portfolio * postive returns should up the portfolio, not drop it more.
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
