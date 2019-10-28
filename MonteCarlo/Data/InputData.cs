using MonteCarlo.Calculator;
using MonteCarlo.Extensions;
using System.Collections.Generic;

namespace MonteCarlo.Data
{
    public class InputData
    {
        public string FilePath_CsvFormatRequirements = "Required Fields (Case-sensitive): 'Year', 'Month', 'CAPE' and all values in DisplayNameToCsvHeader.  A CSV header is expected.  If your data is yearly, month can always be set to '1'.  If CAPE adjustments are not used, CAPE can be set to '1' for all rows.";
        public string FilePath { get; set; }
        public static readonly int InitialAmt = 1_000_000;
        public int NumberOfSimulations { get; set; } = 1000;
        public int PeriodsWithinSimulation { get; set; } = 600;
        public int? StartMonth { get; set; }
        public int? StartYear { get; set; }
        public int? LastYearOfDataUsed { get; set; } = 1975;

        public Dictionary<string, string> DisplayNameToCsvHeader = new Dictionary<string, string>()
        {
            { "Equity", CsvMapper.Equity_Return },
            { "Bond", CsvMapper.Bond_Return },
            { "Cash", CsvMapper.Cash_Return },
            { "Gold", CsvMapper.Gold_Return }
        };

        public List<IPortfolioAdjustments> Adjustments { get; set; } = new List<IPortfolioAdjustments>();
        /// <summary>
        /// Used only for JSON files.
        /// </summary>
        public string AdjustmentTranslation_Documentation = 
            "Set of adjustments used to change outcome.  " +
            "These are all run in the order given.  " +
            "All entries are case sensitive.\n" + 
            "Execution Order:\n" +
            " - Income and Expense adjustments\n" +
            " - Custom Adjustments\n" + 
            " - Adjust income by stock performance\n" +
            "One way of thinking of this is you withdrawal money on the first, " +
            "rebalance or make other adjustments on the same day " +
            "and then realize your gains/losses on the last day of the month.";

        public string AdjustmentTranslation_AdjustByCapeRatio_Documentation =
            "This rebalances the portfolio based upon some simple rules.  We calculate excess = cape - max.  " +
            "If the excess is positive, we divide it by the amount_of_cape_excess_to_increase_adjustment.  " +
            "Then we multiply that by the percentage_to_adjust_by.  " +
            "So for example, say the max cape is 20 and for every additional 5 CAPE over max, you want to adjust by 10%.  " +
            "The adjustment calculation for a cape of 30 would result in the following calculations: " +
            "10 = 30 - 20.  2 = 10/5.  20% = 2 * 10%  That is to say, a 20% adjustment.  " +
            "To take this one step further, if you were moving from stocks to bond and you had 95% bonds," +
            "the 20% adjustment would be limited to only 5%.";

        public List<AdjustmentTranslationData> AdjustmentTranslation { get; set; } = new List<AdjustmentTranslationData>();

        public List<ExpenseOffset> IncomeAndExpenses { get; set; } = new List<ExpenseOffset>();

        public decimal PercentageOfInitialPortolioIsClose { get; set; } = .2M;

        public decimal InitialAmount { get; set; } = InitialAmt;
        public Portfolio Portfolio { get; set; } = new Portfolio() {
            Investments = new Dictionary<string, decimal>()
            {
                {"Equity", 20},
                {"Bond", 5},
                {"Cash", 60 },
                {"Gold", 15 }

            },
            InvestmentAmount = InitialAmt.AsMoney() ,
        };


    }
}
