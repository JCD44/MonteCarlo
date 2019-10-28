using CloneExtensions;
using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Output.Report;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo.Simulations
{
    public abstract class GenericAbstractSimulation : ISimulation
    {
        public abstract IEnumerable<ReturnData> PickReturnDataToUse(List<ReturnData> data, InputData input);
        private static List<ReturnData> Cache = null;

        public OutputMode Mode { get; set; }

        protected virtual List<ReturnData> GetInitalData(InputData input)
        {
            if (Cache == null)
            {
                var data = new CsvParser().GetData(input);
                data = data.Where(a => a.NameToPercentageReturn.Where(b => b.Value.HasValue).Any()).ToList();
                Cache = data.ToList();
            }

            return Cache;
        }

        public List<Outcome> Run(InputData input, List<IPortfolioAdjustments> adjustmentStrategies)
        {
            var list = PickReturnDataToUse(GetInitalData(input), input);

            return Run(list, input, adjustmentStrategies);
        }

        public List<Outcome> Run(IEnumerable<ReturnData> list, InputData input, List<IPortfolioAdjustments> ajustmentStrategies)
        {
            var portfolio = input.Portfolio;
            var outcomes = new List<Outcome>();
            int monthCount = 1;
            foreach (var month in list)
            {
                var work = new PortfolioWork()
                {
                    Portfolio = portfolio,
                    MonthProcessed = monthCount++,
                    ReturnData = month,
                    Adjustments = ajustmentStrategies,
                    Input = input
                };
                var outcome = new PortfolioCalculator().CalculateMonthReturn(work);
                outcomes.Add(outcome);
                //Clone first to make sure the copy is a deep clone... then clear out the temp adjustments.
                portfolio = outcome.NewPortfolio.GetClone();
                portfolio.TemporaryInvestmentAdjustments.Clear();
            }

            outcomes.TrimExcess();

            return outcomes;
        }

        protected List<IPortfolioAdjustments> CreateJsonAdjustments(InputData input)
        {
            List<IPortfolioAdjustments> adjusts = new List<IPortfolioAdjustments>();
            foreach(var item in input.AdjustmentTranslation)
            {
                adjusts.Add(AdjustmentTranslationData.ConvertToAdjustment(item));
            }
            return adjusts;
        }

        public abstract List<OutcomeSummary> ExecuteSimulate(InputData input);
        public abstract void PrintSimulationSpecificDetails(List<OutcomeSummary> list);



        protected virtual void PrintUserSpecifiedData(List<OutcomeSummary> list, InputData input)
        {
            ReportMap[Mode].Report(list, input);
        }

        protected Dictionary<OutputMode, IReport> ReportMap = new Dictionary<OutputMode, IReport>()
        {
            { OutputMode.Files, new CsvFilesReport() }, 
            { OutputMode.GiantCsvConsole, new GiantCsvConsoleReport() }, 
            { OutputMode.GiantCsvFile, new GiantCsvFileReport() }, 
            { OutputMode.Normal, new EmptyReport() },
            { OutputMode.Verbose, new VerboseReport() },
        };


        protected void PrintSummaryData(List<OutcomeSummary> list, InputData input)
        {
            new SummaryDataReport().Report(list, input);

            PrintSimulationSpecificDetails(list);
            PrintUserSpecifiedData(list, input);
        }
        
    }
}
