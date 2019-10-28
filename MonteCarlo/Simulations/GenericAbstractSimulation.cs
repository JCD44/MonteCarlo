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


        //protected virtual Dictionary<string, string> CreateCsvReport(List<OutcomeSummary> list, InputData input)
        //{
        //    var dict = new Dictionary<string, string>();
            
        //    var path = System.IO.Path.GetTempPath();
        //    foreach (var summary in list)
        //    {
        //        var fileName = $"Execution_{summary.RowNumber}_{Guid.NewGuid()}.csv";
        //        var fullPath = path + fileName;
        //        var sb = new StringBuilder();
        //        var investments = summary.Outcomes[0].NewPortfolio.ComputedInvestments();
        //        var returns = summary.Outcomes[0].Returns.Returns;

        //        sb.Append(" \"Simulation Number\",\"Period Number\", \"Month\", \"Year\",");
        //        foreach (var key in investments.Keys)
        //        {
        //            sb.Append($"\"% in {key}\",");
        //        }
        //        foreach (var key in returns.Keys)
        //        {
        //            sb.Append($"\"P/L in {key}\",");
        //        }
        //        sb.Append("\"P/L Total\",\"Returns\", \"Total Other Income & Expenses\",");

        //        sb.AppendLine("");
        //        foreach (var item in summary.Outcomes)
        //        {
        //            sb.Append($"\"{summary.RowNumber}\", \"{item.PeriodNumber}\", \"{item.ReturnDataDate.Month}\", \"{item.ReturnDataDate.Year}\", ");
        //            investments = item.NewPortfolio.ComputedInvestments();
        //            returns = item.Returns.Returns;

        //            foreach (var val in investments.Values)
        //            {
        //                sb.Append($"\"{val.MakeReadable()}\",");
        //            }

        //            foreach (var val in returns.Values)
        //            {
        //                sb.Append($"\"{val.MakeReadable()}\",");
        //            }

        //            sb.Append($"\"{item.Returns.MonthProfitOrLoss.MakeReadable()}\",");
        //            sb.Append($"\"{item.NewPortfolio.InvestmentAmount.MakeReadable()}\",");
        //            sb.Append($"\"{item.TotalIncomeAndExpenses.MakeReadable()}\",");

        //            sb.AppendLine("");
        //        }
        //        dict.Add(fullPath, sb.ToString()); 
        //    }

        //    return dict;
        //}

        protected virtual void PrintUserSpecifiedData(List<OutcomeSummary> list, InputData input)
        {
            ReportMap[Mode].Report(list, input);
            //if (Mode == OutputMode.Files)
            //{
            //    ReportMap[Mode].Report(list, input);

            //    //foreach(var item in CreateCsvReport(list, input)) { 
            //    //    WriteManager.Write($"Writing data to {item.Key}");
            //    //    System.IO.File.WriteAllText(item.Key, item.Value);
            //    //}
            //}

            //if (Mode == OutputMode.GiantCsvFile)
            //{
            //    ReportMap[Mode].Report(list, input);

            //    //var filename = $"Execution_{Guid.NewGuid()}.csv";
            //    //var path = System.IO.Path.GetTempPath();
            //    //var fullPath = path + filename;

            //    //var text = CreateGiantCsv(list, input);

            //    //WriteManager.Write($"Writing data to {fullPath}");
            //    //System.IO.File.WriteAllText(fullPath, text);
            //}
            //if (Mode == OutputMode.GiantCsvConsole)
            //{
            //    ReportMap[Mode].Report(list, input);

            //    ////I'm imagining a time where an external program like a website call the exe and wants only this output...
            //    //WriteManager.Writer = new ConsoleWriter();
            //    //var text = CreateGiantCsv(list, input);
            //    //WriteManager.Write(text);
            //}
        }

        protected Dictionary<OutputMode, IReport> ReportMap = new Dictionary<OutputMode, IReport>()
        {
            { OutputMode.Files, new CsvFilesReport() }, 
            { OutputMode.GiantCsvConsole, new GiantCsvConsoleReport() }, 
            { OutputMode.GiantCsvFile, new GiantCsvFileReport() }, 
            { OutputMode.Normal, new EmptyReport() },
            { OutputMode.Verbose, new VerboseReport() },
        };

        //protected virtual string CreateGiantCsv(List<OutcomeSummary> list, InputData input)
        //{
        //    var sb = new StringBuilder();
        //    var first = true;
        //    foreach (var item in CreateCsvReport(list, input))
        //    {
        //        if (first)
        //        {
        //            sb.Append(item.Value);
        //        }
        //        else
        //        {
        //            sb.Append(item.Value.Substring(item.Value.IndexOf(Environment.NewLine) + Environment.NewLine.Length));
        //        }
        //        first = false;
        //    }

        //    return sb.ToString();
        //}
        //protected virtual void PrintVerboseData(List<OutcomeSummary> list, InputData input)
        //{
        //    if (Mode == OutputMode.Verbose)
        //    {
        //        ReportMap[Mode].Report(list, input);
        //        //foreach (var summary in list)
        //        //{
        //        //    WriteManager.Write($"***********Run # {summary.RowNumber}***********");
        //        //    WriteManager.Write("-----");
        //        //    WriteManager.Write($"Started: {summary.PortolioStartTime.PrettyDate()} ");
        //        //    WriteManager.Write($"- Is Failure: {summary.IsFail}");
        //        //    WriteManager.Write($"- Is Close: {summary.IsClose}");
        //        //    WriteManager.Write($"- Worst Month: {summary.WorstMonth}");
        //        //    WriteManager.Write($"- Worst Amount: {summary.WorstAmount.MakeReadable()}");
        //        //    WriteManager.Write($"- Total Returns By Category: {summary.TotalReturnsByCategory.DictToString()}");
        //        //    WriteManager.Write("-----");

        //        //    var period = 0;
        //        //    foreach (var item in summary.Outcomes)
        //        //    {
        //        //        period++;
        //        //        WriteManager.Write($"Period: {period} - {item}");
        //        //    }
        //        //    WriteManager.Write($"***********End of Run # {summary.RowNumber}***********");
        //        //}
        //    }
        //}

        protected void PrintSummaryData(List<OutcomeSummary> list, InputData input)
        {
            new SummaryDataReport().Report(list, input);

            PrintSimulationSpecificDetails(list);
            //PrintVerboseData(list, input);
            PrintUserSpecifiedData(list, input);
        }
        
    }
}
