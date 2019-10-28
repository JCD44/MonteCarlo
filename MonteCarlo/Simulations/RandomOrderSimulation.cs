using CloneExtensions;
using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using MonteCarlo.Output;
using MonteCarlo.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public class RandomOrderSimulation : GenericAbstractSimulation
    {
        public override List<OutcomeSummary> ExecuteSimulate(InputData input)
        {
            var list = new List<OutcomeSummary>();
            Parallel.For(0, input.NumberOfSimulations, index =>
            {

                var outcome = new RandomOrderSimulation().Run(input.GetClone());
                var summary = new OutcomeSummary()
                {
                    Outcomes = outcome,
                    RowNumber = index
                };
                summary.ProcessOutcomes(input);
                lock (list)
                {
                    list.Add(summary);
                }
            });

            PrintSummaryData(list, input);

            return list;
        }

        public override IEnumerable<ReturnData> PickReturnDataToUse(List<ReturnData> data, InputData input)
        {
            return data.Where(d=>d.Year<=input.LastYearOfDataUsed).PickRandom(input.PeriodsWithinSimulation).ToList();
        }

        public override void PrintSimulationSpecificDetails(List<OutcomeSummary> list)
        {
            WriteManager.Write($" * Failure Details: \n  - {string.Join("\n  - ", list.Where(a => a.IsFail).OrderBy(b => b.IsCloseFirstMonth).Select(b => $" Initial Date: {b.PortolioStartTime.PrettyDate()}, First Close Month: {b.IsCloseFirstMonth}, First Failure Month: {b.FirstFailureMonth}, Worst Month: {b.WorstMonth}, Worst Amount: {b.WorstAmount.MakeReadable()}").ToList())}");
            WriteManager.Write($" * Close Details: \n  - {string.Join("\n  - ", list.Where(a => a.IsClose && !a.IsFail).OrderBy(b => b.IsCloseFirstMonth).Select(b => $" Initial Date: {b.PortolioStartTime.PrettyDate()}, First Close Month: {b.IsCloseFirstMonth}, Worst Month: {b.WorstMonth}, Worst Amount: {b.WorstAmount.MakeReadable()}").ToList())}");
            WriteManager.Write($" * Top 10 Worst Details: \n  - {string.Join("\n  - ", list.OrderBy(a => a.WorstAmount).Take(10).Select(b => $" Initial Date: {b.PortolioStartTime.PrettyDate()}, Worst Month: {b.WorstMonth}, Worst Amount: {b.WorstAmount.MakeReadable()}").ToList())}");
        }

        public List<Outcome> Run(InputData input)
        {
            var adjust = new List<IPortfolioAdjustments>();
            adjust.Add(new AdjustByIncomeAndExpenses(input.IncomeAndExpenses));
            adjust.AddRange(CreateJsonAdjustments(input));
            adjust.Add(new AdjustIncomeByStockPerformance());

            return Run(input, adjust);

        }
    }
}
