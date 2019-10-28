using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class AdjustByIncomeAndExpenses : IPortfolioAdjustments
    {
        public List<ExpenseOffset> Offsets { get; set; }
        public AdjustByIncomeAndExpenses(List<ExpenseOffset> offsets)
        {
            this.Offsets = offsets;
        }

        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            foreach (var item in Offsets) {
                if (item.StartMonth > portfolio.MonthProcessed) continue;
                if (item.EndMonth != null && item.EndMonth < portfolio.MonthProcessed) continue;

                portfolio.TotalIncomeAndExpenses += item.OffsetAmount;
                portfolio.Portfolio.InvestmentAmount += item.OffsetAmount;
            }

            return portfolio;
        }
        public void Init(PortfolioWork portfolio)
        {
        }
    }
}
