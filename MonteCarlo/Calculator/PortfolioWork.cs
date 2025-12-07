using CloneExtensions;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using NodaMoney;
using System.Collections.Generic;

namespace MonteCarlo.Calculator
{
    public class PortfolioWork
    {
        public Portfolio Portfolio { get; set; }
        public ReturnData ReturnData { get; set; }
        public List<IPortfolioAdjustments> Adjustments { get; set; }
        public int MonthProcessed { get; set; }
        public InputData Input { get; set; }
        public MonthlyReturn Returns { get; set; } = new MonthlyReturn();
        public Money TotalIncomeAndExpenses { get { return TotalExpense + TotalIncome; } }
        public Money TotalExpense { get; set; } = 0M.AsMoney();
        public Money TotalIncome { get; set; } = 0M.AsMoney();

        public Portfolio CopyOfPortfolio()
        {
            return Portfolio.GetClone();
        }
    }
}
