using MonteCarlo.Extensions;
using Narvalo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data
{
    public class Outcome
    {
        public Portfolio PreviousPortfolio { get; set; }
        public Portfolio NewPortfolio { get; set; }
        public int PeriodNumber { get; set; }
        public DateTime ReturnDataDate { get; set; }
        public MonthlyReturn Returns { get; set; }
        public Money TotalIncomeAndExpenses { get; set; }

        public override string ToString()
        {
            return $"{ReturnDataDate.PrettyDate()} - {PreviousPortfolio.InvestmentAmount.MakeReadable()} changed to {NewPortfolio.InvestmentAmount.MakeReadable()} - Period P/L: {Returns.MonthProfitOrLoss} - Returns P/L: {Returns.Returns.DictToString()}";
        }

    }
}
