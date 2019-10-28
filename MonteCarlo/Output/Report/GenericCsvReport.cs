using System;
using System.Collections.Generic;
using System.Text;
using MonteCarlo.Data;
using MonteCarlo.Extensions;

namespace MonteCarlo.Output.Report
{
    public abstract class GenericCsvReport : IReport
    {
        public abstract void Report(List<OutcomeSummary> list, InputData input);

        protected virtual Dictionary<string, string> CreateCsvReport(List<OutcomeSummary> list, InputData input)
        {
            var dict = new Dictionary<string, string>();

            var path = System.IO.Path.GetTempPath();
            foreach (var summary in list)
            {
                var fileName = $"Execution_{summary.RowNumber}_{Guid.NewGuid()}.csv";
                var fullPath = path + fileName;
                var sb = new StringBuilder();
                var investments = summary.Outcomes[0].NewPortfolio.ComputedInvestments();
                var returns = summary.Outcomes[0].Returns.Returns;

                sb.Append(" \"Simulation Number\",\"Period Number\", \"Month\", \"Year\",");
                foreach (var key in investments.Keys)
                {
                    sb.Append($"\"% in {key}\",");
                }
                foreach (var key in returns.Keys)
                {
                    sb.Append($"\"P/L in {key}\",");
                }
                sb.Append("\"P/L Total\",\"Returns\", \"Total Other Income & Expenses\",");

                sb.AppendLine("");
                foreach (var item in summary.Outcomes)
                {
                    sb.Append($"\"{summary.RowNumber}\", \"{item.PeriodNumber}\", \"{item.ReturnDataDate.Month}\", \"{item.ReturnDataDate.Year}\", ");
                    investments = item.NewPortfolio.ComputedInvestments();
                    returns = item.Returns.Returns;

                    foreach (var val in investments.Values)
                    {
                        sb.Append($"\"{val.MakeReadable()}\",");
                    }

                    foreach (var val in returns.Values)
                    {
                        sb.Append($"\"{val.MakeReadable()}\",");
                    }

                    sb.Append($"\"{item.Returns.MonthProfitOrLoss.MakeReadable()}\",");
                    sb.Append($"\"{item.NewPortfolio.InvestmentAmount.MakeReadable()}\",");
                    sb.Append($"\"{item.TotalIncomeAndExpenses.MakeReadable()}\",");

                    sb.AppendLine("");
                }
                dict.Add(fullPath, sb.ToString());
            }

            return dict;
        }

        protected virtual string CreateGiantCsv(List<OutcomeSummary> list, InputData input)
        {
            var sb = new StringBuilder();
            var first = true;
            foreach (var item in CreateCsvReport(list, input))
            {
                if (first)
                {
                    sb.Append(item.Value);
                }
                else
                {
                    sb.Append(item.Value.Substring(item.Value.IndexOf(Environment.NewLine) + Environment.NewLine.Length));
                }
                first = false;
            }

            return sb.ToString();
        }


    }
}
