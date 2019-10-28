using MonteCarlo.Data;
using MonteCarlo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Test.TestData
{
    public class PortfolioTestData
    {
        public static Portfolio CreateData()
        {
            return new Portfolio()
            {
                InvestmentAmount = 90100.AsMoney(),
                Investments = new Dictionary<string, decimal>()
                {
                    {"Bond", .5m },
                    {"Equity", .5m },
                },
            };
        }

        public static Portfolio CreateDataMoreEquity()
        {
            return new Portfolio()
            {
                InvestmentAmount = 90100.AsMoney(),
                Investments = new Dictionary<string, decimal>()
                {
                    {"Bond", .25m },
                    {"Equity", .75m },
                },
            };
        }

        public static Portfolio CreateDataMoreBonds()
        {
            return new Portfolio()
            {
                InvestmentAmount = 90000.AsMoney(),
                Investments = new Dictionary<string, decimal>()
                {
                    {"Bond", .75m },
                    {"Equity", .25m },
                },
            };
        }

    }
}
