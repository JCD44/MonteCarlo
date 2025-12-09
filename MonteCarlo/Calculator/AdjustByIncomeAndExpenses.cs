using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class AdjustByIncomeAndExpenses : IPortfolioAdjustments
    {
        public List<ExpenseOffset> Offsets { get; set; }
        public int StartMonth { get; set; } = 0;
        public int? EndMonth { get; set; } = null;
        public AdjustByIncomeAndExpenses(List<ExpenseOffset> offsets)
        {
            this.Offsets = offsets;
        }

        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            if (!AdjustHelper.ShouldExecute(this, portfolio)) return portfolio;

            foreach (var item in Offsets) {
                if (!item.ShouldOffset(portfolio)) continue;

                //Track income vs expense for reporting.
                if (item.OffsetAmount.ToDecimal() > 0) portfolio.TotalIncome += item.OffsetAmount;
                else portfolio.TotalExpense += item.OffsetAmount;
                portfolio.Portfolio.InvestmentAmount += item.OffsetAmount;
            }

            return portfolio;
        }
        public void Init(PortfolioWork portfolio)
        {
        }
    }
}
