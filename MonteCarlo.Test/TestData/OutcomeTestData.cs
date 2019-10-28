using MonteCarlo.Data;
using MonteCarlo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Test.TestData
{
    public class OutcomeTestData
    {
        public static List<Outcome> CreateData()
        {
            var list = new List<Outcome>();
            var o = CreateOutcomeWithoutReturn(0, 1);
            o.Returns.Returns.Add("Equity", 50.AsMoney());
            o.Returns.Returns.Add("Bond", 50.AsMoney());
            list.Add(o);

            var o1 = CreateOutcomeWithoutReturn(1, 2, PortfolioTestData.CreateDataMoreBonds(), PortfolioTestData.CreateDataMoreEquity());
            o1.Returns.Returns.Add("Equity", 75.AsMoney());
            o1.Returns.Returns.Add("Bond", 25.AsMoney());
            list.Add(o1);


            var o2 = CreateOutcomeWithoutReturn(2, 3, PortfolioTestData.CreateDataMoreEquity(), PortfolioTestData.CreateDataMoreBonds());
            o2.Returns.Returns.Add("Equity", -25.AsMoney());
            o2.Returns.Returns.Add("Bond", -75.AsMoney());
            o2.Returns.MonthProfitOrLoss = -100.AsMoney();
            list.Add(o2);

            return list;
        }

        private static Outcome CreateOutcomeWithoutReturn(int months, int period)
        {
            return CreateOutcomeWithoutReturn(months, period, PortfolioTestData.CreateData(), PortfolioTestData.CreateDataMoreBonds());
        }

        private static Outcome CreateOutcomeWithoutReturn(int months, int period, Portfolio prevPort, Portfolio newPort)
        {
            return new Outcome()
            {
                PreviousPortfolio = prevPort,
                NewPortfolio = newPort,
                PeriodNumber = period,
                ReturnDataDate = DateTime.Now.AddYears(-10).AddMonths(months),
                Returns = new MonthlyReturn()
                {
                    MonthProfitOrLoss = 100.AsMoney(),
                },
                TotalIncomeAndExpenses = 1000.AsMoney()
            };
        }

    }
}
