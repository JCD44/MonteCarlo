using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Data.JSON;
using MonteCarlo.Extensions;
using MonteCarlo.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonteCarlo.Dispatch
{
    class DispatchCreateFile : IDispatchProgram
    {
        public void Execute(GenericArgsParser args)
        {
            var input = new InputData();

            input.IncomeAndExpenses.Add(
                new ExpenseOffset() { OffsetAmount = (((InputData.InitialAmt * .043M) / 12) * -1).AsMoney() });


            input.IncomeAndExpenses.Add(new ExpenseOffset()
            {
                StartMonth = 380,
                OffsetAmount = 1099.AsMoney(),
            });

            input.IncomeAndExpenses.Add(new ExpenseOffset()
            {
                StartMonth = 40,
                EndMonth = 200,
                OffsetAmount = 800.AsMoney(),
            });

            input.FilePath = "";
            input.Adjustments.Clear();

            input.AdjustmentTranslation = new List<AdjustmentTranslationData>()
            {
                new AdjustmentTranslationData()
                {
                    Type = "AdjustByGlide",
                    NameToStringValue = new Dictionary<string, List<string>>
                    {
                        {"from", new List<string>() { "Bond" } },
                        {"to", new List<string>() { "Equity" } },
                    },
                    NameToDecimalValue = new Dictionary<string, decimal>()
                    {
                        { "glide", 5m/12 }
                    }
                },

                new AdjustmentTranslationData() {
                    Type = "AdjustByMaxValues",
                    NameToDecimalValue = new Dictionary<string, decimal>()
                    {
                        { "Equity", 75 }
                    }
                },
                new AdjustmentTranslationData()
                {
                    Type = "AdjustByMinValues",
                    NameToDecimalValue = new Dictionary<string, decimal>()
                    {
                        { "Bond", 5 },
                        { "Equity", 20 },
                        { "Gold", 15 },
                    }
                },
                new AdjustmentTranslationData()
                {
                    Type = "AdjustByCapeRatio",
                    NameToStringValue = new Dictionary<string, List<string>>
                    {
                        {"from", new List<string>() { "Bond" } },
                        {"to", new List<string>() { "Equity" } },
                    },
                    NameToDecimalValue = new Dictionary<string, decimal>()
                    {
                        {"max_cape_before_action",  20},
                        {"percentage_to_adjust_by",  .2M},
                        {"amount_of_cape_excess_to_increase_adjustment",  10M}
                    }
                }



            };

            Input.ToFile(input, args.InputFilePath);

            WriteManager.Write("Execution Completed.");
        }
    }
}
