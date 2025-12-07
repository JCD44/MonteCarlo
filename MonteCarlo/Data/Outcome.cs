using MonteCarlo.Extensions;
using NodaMoney;
using System;

namespace MonteCarlo.Data
{
    public class Outcome
    {
        public Portfolio PreviousPortfolio { get; set; }
        public Portfolio NewPortfolio { get; set; }
        public int PeriodNumber { get; set; }
        public DateTime ReturnDataDate { get; set; }
        public MonthlyReturn Returns { get; set; }
        public Money TotalIncomeAndExpenses { get { return TotalExpense + TotalIncome; } }
        public Money TotalExpense { get; set; } = 0M.AsMoney();
        public Money TotalIncome { get; set; } = 0M.AsMoney();


        public override string ToString()
        {
            return $"{ReturnDataDate.PrettyDate()} - {PreviousPortfolio.InvestmentAmount.MakeReadable()} changed to {NewPortfolio.InvestmentAmount.MakeReadable()} - Period P/L: {Returns.MonthProfitOrLoss} - Returns P/L: {Returns.Returns.DictToString()}";
        }

    }
}
