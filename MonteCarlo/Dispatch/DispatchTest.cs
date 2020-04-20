using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Data.JSON;
using MonteCarlo.Extensions;
using MonteCarlo.Simulations;
using System.Collections.Generic;

namespace MonteCarlo.Dispatch
{
    public class DispatchTest : IDispatchProgram
    {

        public static readonly Dictionary<string, ISimulation> Simulation = new Dictionary<string, ISimulation>()
        {
            { "AllYears", new AllYearsSimulation() },
            { "RandomOrder", new RandomOrderSimulation() }
        };
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
            input.AdjustmentTranslation = new List<AdjustmentTranslationData>()
            {
                new AdjustmentTranslationData() {
                    Type = "AdjustByDoingNothing"
                },
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
                        { "glide", 5m/12m }
                    }
                },

                new AdjustmentTranslationData() {
                    Type = "AdjustByMaxMinValues"
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

            var path = @"c:\tmp\Input.json";

            Input.ToFile(input, path);

            var data = Input.FromFileSafe(path, null);


            var comp = data.Compare(input);


            Simulation["AllYears"].ExecuteSimulate(input);
        }
    }
}
