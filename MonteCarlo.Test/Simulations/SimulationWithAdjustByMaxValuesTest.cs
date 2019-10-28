﻿using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo.Test.Simulations
{
    public class SimulationWithAdjustByMaxValuesTest
    {

        public IEnumerable<ReturnData> CreateReturnData()
        {
            var list = new List<ReturnData>()
            {
                CreateReturn(1),
                CreateReturn(2),
                CreateReturn(3),
                CreateReturn(4),
                CreateReturn(5),
                CreateReturn(6),
                CreateReturn(7),
                CreateReturn(8),
                CreateReturn(9),
                CreateReturn(10),
                CreateReturn(11, 10),
                CreateReturn(12),

            };

            return list;
        }

        public ReturnData CreateReturn(int row, decimal cape = 30)
        {
            return new ReturnData()
            {
                CAPE = cape,
                Month = 1,
                Year = 1929 + row,
                Row = row,
                NameToPercentageReturn = new Dictionary<string, decimal?>
                    {
                        { "Bond", .10m },
                        { "Equity", .5m },
                    }
            };
        }

        public InputData CreateInputData()
        {
            var initialAmount = 100;
            var data = new InputData()
            {
                Portfolio = new Portfolio()
                {
                    Investments = new Dictionary<string, decimal>()
                    {
                        {"Equity", 50},
                        {"Bond", 50},

                    },
                    InvestmentAmount = initialAmount.AsMoney(),
                },
                InitialAmount = initialAmount,
                Adjustments = new List<IPortfolioAdjustments>(),
                IncomeAndExpenses = new List<ExpenseOffset>()
                {

                },
            };

            return AddAdjustments(new AdjustByMaxMinValues(null, new Dictionary<string, decimal>()
                    {
                        { "Bond", 60 },
                        { "Equity", 0 }
                    }), data);
        }

        private InputData AddAdjustments(IPortfolioAdjustments adjustment, InputData data)
        {
            data.Adjustments.Add(new AdjustByIncomeAndExpenses(data.IncomeAndExpenses));
            data.Adjustments.Add(new AdjustByCapeRatio("Equity", "Bond", 20, 10, 10));
            data.Adjustments.Add(adjustment);
            data.Adjustments.Add(new AdjustIncomeByStockPerformance());

            return data;
        }

        [Test]
        public void VerifyOffsetIsCalculatedCorrectly()
        {
            var sim = new AllYearsSimulation();
            var input = CreateInputData();
            var result = sim.Run(CreateReturnData(), input, input.Adjustments);

            Assert.IsNotNull(result);

            foreach (var item in result[0].NewPortfolio.ComputedInvestments())
            {
                Assert.AreEqual(result[result.Count - 1].NewPortfolio.ComputedInvestments()[item.Key], item.Value, $"{item.Key}");
            }

        }

        [Test]
        public void VerifyOffsetIsCalculatedCorrectlyWithExcessOffset()
        {
            var sim = new AllYearsSimulation();
            var input = CreateInputData();
            input.Adjustments.Clear();
            input = AddAdjustments(new AdjustByMaxMinValues(null, new Dictionary<string, decimal>()
                    {
                        { "Bond", 120 },
                        { "Equity", 0 }
                    }), input);
            var result = sim.Run(CreateReturnData(), input, input.Adjustments);

            Assert.IsNotNull(result);

            Assert.AreEqual(100, result[0].NewPortfolio.ComputedInvestments().Sum(a => a.Value));
            

        }
    }
}
