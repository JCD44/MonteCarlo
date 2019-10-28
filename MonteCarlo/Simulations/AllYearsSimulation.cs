using CloneExtensions;
using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using MonteCarlo.Output;
using MonteCarlo.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo
{
    public class AllYearsSimulation : GenericAbstractSimulation
    {

        private InputData CloneInputData(InputData input)
        {
            var inputCloneInit = new Dictionary<Type, Func<object, object>>
            {
                { typeof(IPortfolioAdjustments), (s) => new AdjustByDoingNothing() }
            };

            var tmpInput = input.GetClone(inputCloneInit);
            tmpInput.Adjustments.Clear();
            foreach (var item in tmpInput.Adjustments)
            {
                tmpInput.Adjustments.Add(item.GetClone());
            }

            return tmpInput;

        }

        public override List<OutcomeSummary> ExecuteSimulate(InputData input)
        {
            var list = new List<OutcomeSummary>();

            var date = new DateTime(1800, 1, 1);
            var rowNumber = 0;
            while (date < DateTime.Now.AddMonths(1))
            {
                date = date.AddMonths(1);

                //Exclude all years filtered by start year/month.
                if (input.StartYear > date.Year) continue;
                if (input.StartYear == date.Year && input.StartMonth > date.Month) continue;
                if (input.LastYearOfDataUsed < date.Year) continue;

                var tmpInput = CloneInputData(input);

                tmpInput.StartMonth = date.Month;
                tmpInput.StartYear = date.Year;

                var outcome = new AllYearsSimulation().Run(tmpInput);
                rowNumber++;
                var summary = new OutcomeSummary() { Outcomes = outcome, RowNumber = rowNumber,  };
                summary.ProcessOutcomes(tmpInput);
                lock (list)
                {
                    list.Add(summary);
                }
            }

            PrintSummaryData(list, input);

            return list;
        }

        public override IEnumerable<ReturnData> PickReturnDataToUse(List<ReturnData> data, InputData input)
        {
            var skip = 0;
            if (input.StartMonth != null) skip = (int)input.StartMonth - 1;
            var list = data.OrderBy(a=>a.Row).Where(a => a.Year >= input.StartYear).Skip(skip).ToList();
            var additional = input.PeriodsWithinSimulation - list.Count;
            if (additional > 0) list.AddRange(data.PickRandom(additional));

            return list.Take(input.PeriodsWithinSimulation);
        }

        public override void PrintSimulationSpecificDetails(List<OutcomeSummary> list)
        {
            WriteManager.Write($" * Failure Details: \n  - {string.Join("\n  - ", list.Where(a => a.IsFail).OrderBy(b => b.PortolioStartTime).Select(b => $" Initial Date: {b.PortolioStartTime.PrettyDate()}, First Close Month: {b.IsCloseFirstMonth}, First Failure Month: {b.FirstFailureMonth}, Worst Month: {b.WorstMonth}, Worst Amount: {b.WorstAmount.MakeReadable()}").ToList())}");
            WriteManager.Write($" * Close Details: \n  - {string.Join("\n  - ", list.Where(a => a.IsClose && !a.IsFail).OrderBy(b => b.PortolioStartTime).Select(b => $" Initial Date: {b.PortolioStartTime.PrettyDate()}, First Close Month: {b.IsCloseFirstMonth}, Worst Month: {b.WorstMonth}, Worst Amount: {b.WorstAmount.MakeReadable()}").ToList())}");
            WriteManager.Write($" * Top 10 Worst Details: \n  - {string.Join("\n  - ", list.OrderBy(a => a.WorstAmount).Take(10).Select(b => $" Initial Date: {b.PortolioStartTime.PrettyDate()}, Worst Month: {b.WorstMonth}, Worst Amount: {b.WorstAmount.MakeReadable()}").ToList())}");
        }

        public List<Outcome> Run(InputData input)
        {
            //This should be true for all json based files.
           if(input.Adjustments==null || input.Adjustments.Count==0)
            {
                var adjust = new List<IPortfolioAdjustments>();
                adjust.Add(new AdjustByIncomeAndExpenses(input.IncomeAndExpenses));
                adjust.AddRange(CreateJsonAdjustments(input));
                adjust.Add(new AdjustIncomeByStockPerformance());
                input.Adjustments.AddRange(adjust
                        //new AdjustByCapeRatio("Equity", "Bond"),
                        //new AdjustByGlide("Cash", "Equity"),
                        //new AdjustByMaxMinValues(),
                        
                    );
            }

            return Run(input, input.Adjustments);

        }
    }
}
