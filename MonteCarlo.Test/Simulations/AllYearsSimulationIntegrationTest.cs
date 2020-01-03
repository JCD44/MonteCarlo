using KellermanSoftware.CompareNetObjects;
using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonteCarlo.Test.Simulations
{
    public class AllYearsSimulationIntegrationTestTest
    {
        [SetUp]
        public void Setup()
        {
            //The file is based upon: https://earlyretirementnow.com/2017/01/25/the-ultimate-guide-to-safe-withdrawal-rates-part-7-toolbox/
            //Requires 16 decimal numbers in order to match exact values.
            if (!System.IO.File.Exists(Path)) throw new Exception($"Unable to run tests, dependent file \"{Path}\" does not exist.");
        }
        //private static string FileName { get; set; } = "Stock_Bond_Returns_new.csv";

        private static string FileName { get; set; } = "Stock_Bond_Returns2.csv";
        private string Path
        {
            get
            {
                return
                    System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\..\..\..\..\MonteCarlo")
                    + $"{System.IO.Path.DirectorySeparatorChar}{FileName}";
            }
        }

        [Test]
         public void AllYearsProcessesCorrectly()
        {
            var input = new InputData()
            {
                PeriodsWithinSimulation = 50,
                LastYearOfDataUsed = 1939,
                StartYear = 1930,
            };
            input.IncomeAndExpenses.Add(new ExpenseOffset()
            {
                StartMonth = 380,
                OffsetAmount = 1050.AsMoney(),
            });

            input.FilePath = Path;

            var sim = new AllYearsSimulation();

            var result1 = sim.ExecuteSimulate(input);
            var result2 = sim.ExecuteSimulate(input);

            CompareLogic compareLogic = new CompareLogic();
                
            var comp = compareLogic.Compare(result1, result2);
            Assert.IsTrue(comp.AreEqual, "Verify data is consistently processed. Diff: " + comp.DifferencesString);
            Assert.AreEqual(120, result1.Count, "Verify only 120 (12 months * 10 years) months were executed."); 
        }

        [Test]
        public void AllYearsProcessSameAsExcel()
        {
            var initialAmt = 630_000;
            var incomeAndExpenseList = new List<ExpenseOffset> {
                new ExpenseOffset() { OffsetAmount = (((initialAmt * .043M)/12)*-1).AsMoney()}
            };
            var input = new InputData()
            {
                PeriodsWithinSimulation = 600,
                LastYearOfDataUsed = 1905,
                StartYear = 1905,
                InitialAmount = initialAmt,
                IncomeAndExpenses = incomeAndExpenseList,
                Portfolio = new Portfolio()
                {
                    InvestmentAmount = initialAmt.AsMoney(),
                    Investments = new Dictionary<string, decimal>()
                    {
                        {"Bond", 25 },
                        {"Equity", 25 },
                        {"Gold", 25 },
                        {"Cash", 25 }
                    }
                },
                Adjustments =new  List<IPortfolioAdjustments>() {
                    new AdjustByIncomeAndExpenses(incomeAndExpenseList),
                    new AdjustIncomeByStockPerformance()
                },


            };
            input.IncomeAndExpenses.Add(new ExpenseOffset()
            {
                StartMonth = 360,
                OffsetAmount = 1050.AsMoney(),
            });
            input.FilePath = Path;
            var sim = new AllYearsSimulation();

            var result1 = sim.ExecuteSimulate(input);

            input.DisplayNameToCsvHeader = new Dictionary<string, string>()
                {
                    { "Equity", CsvMapper.Equity_Return.Replace(" Return","") },
                    { "Bond", "10Y BM" },
                    { "Cash", CsvMapper.Cash_Return.Replace(" Return","") },
                    { "Gold", CsvMapper.Gold_Return.Replace(" Return","") }
                };

            //TODO Remove this test code used to compare two different spreadsheets.
            //FileName = "Stock_Bond_Returns_new.csv";
            //input.FilePath = Path;

            //var result2 = sim.ExecuteSimulate(input);

            //CompareLogic compareLogic = new CompareLogic()
            //{
            //    Config = new ComparisonConfig()
            //    {
            //        MembersToIgnore = new List<string>()
            //        {
            //            "Input",
            //        },
            //    }
            //};

            //var comp = compareLogic.Compare(result1, result2);
            
            //Assert.IsTrue(comp.AreEqual, "Verify data is consistently processed. Diff: " + comp.DifferencesString);

            Assert.AreEqual(12, result1.Count, "Verify only 12 months were executed.");
            //Calculated based upon big ern spreadsheet model.
            //Used from old spreadsheet.  New spreadsheet gives new value I need to validate again...
            Assert.AreEqual(-614296.48.AsMoney().Normalize(MidpointRounding.AwayFromZero), result1[0].Outcomes[599].NewPortfolio.InvestmentAmount.Normalize(MidpointRounding.AwayFromZero));
            //Assert.AreEqual(-614296.48.AsMoney().Normalize(MidpointRounding.AwayFromZero), result2[0].Outcomes[599].NewPortfolio.InvestmentAmount.Normalize(MidpointRounding.AwayFromZero));


        }

    }
}