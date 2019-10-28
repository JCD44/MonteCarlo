using CloneExtensions;
using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class PortfolioCalculator
    {

        private static readonly PortfolioAdjuster adjuster = new PortfolioAdjuster();

        public Outcome CalculateMonthReturn(PortfolioWork portfolio)
        {
            var outcome = new Outcome
            {
                PreviousPortfolio = portfolio.CopyOfPortfolio(),
                NewPortfolio = adjuster.UpdatePortfolio(portfolio).CopyOfPortfolio(),
                PeriodNumber = portfolio.MonthProcessed,
                ReturnDataDate = new DateTime((int)portfolio.ReturnData.Year, (int) portfolio.ReturnData.Month, 1),
                Returns = portfolio.Returns,
                TotalIncomeAndExpenses = portfolio.TotalIncomeAndExpenses,
            };


            return outcome;
        }
    }
}
