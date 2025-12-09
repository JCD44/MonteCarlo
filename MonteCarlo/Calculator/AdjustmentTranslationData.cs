using MonteCarlo.Data;
using System;
using System.Collections.Generic;

namespace MonteCarlo.Calculator
{
    public class AdjustmentTranslationData
    {
        public string Type { get; set; }
        public Dictionary<string, List<string>> NameToStringValue { get; set; } 
            = new Dictionary<string, List<string>>();
        public Dictionary<string, Decimal> NameToDecimalValue { get; set; }
            = new Dictionary<string, Decimal>();

        public int StartMonth { get; set; } = 0;
        public int? EndMonth { get; set; } = null;

        public static IPortfolioAdjustments ConvertToAdjustment(AdjustmentTranslationData adjustment)
        {
            var type = adjustment.Type;
            IPortfolioAdjustments adjustBy;
            switch (type)
            {
                case "AdjustByGlide":
                    var from = adjustment.NameToStringValue["from"][0];
                    var to = adjustment.NameToStringValue["to"][0];
                    adjustBy = new AdjustByGlide(from, to, adjustment.NameToDecimalValue["glide"]);
                    break;
                case "AdjustByDoingNothing":
                    //No need to use the Start/EndMonth, so just return.
                    return new AdjustByDoingNothing();
                //case "AdjustByIncomeAndExpenses":
                //    return new AdjustByIncomeAndExpenses(adjustment.ExpenseOffsets);
                case "AdjustByMaxValues":
                    var max = adjustment.NameToDecimalValue;
                    adjustBy = new AdjustByMaxMinValues(null, max);
                    break;
                case "AdjustByMinValues":
                    var min = adjustment.NameToDecimalValue;

                    adjustBy = new AdjustByMaxMinValues(min, null);
                    break;
                case "AdjustIncomeByStockPerformance":
                    adjustBy = new AdjustIncomeByStockPerformance();
                    break;
                case "AdjustByCapeRatio":
                    var from1 = adjustment.NameToStringValue["from"][0];
                    var to1 = adjustment.NameToStringValue["to"][0];
                    var maxCapeBeforeAction = SafeReadValue(adjustment.NameToDecimalValue, "max_cape_before_action");
                    var percentageAdjusted = SafeReadValue(adjustment.NameToDecimalValue, "percentage_to_adjust_by");
                    var additionalAdjustmentPerCapeExcess = SafeReadValue(adjustment.NameToDecimalValue, "amount_of_cape_excess_to_increase_adjustment");

                    // (capeOverage / amountOfAdditionalCapePerAdjustment) * percentageAdjustment

                    adjustBy = new AdjustByCapeRatio(from1, to1, maxCapeBeforeAction, percentageAdjusted, additionalAdjustmentPerCapeExcess);
                    break;
                default:
                    throw new Exception($"Type '{type}' is not currently supported.");
            }

            adjustBy.StartMonth = adjustment.StartMonth;
            adjustBy.EndMonth = adjustment.EndMonth;

            return adjustBy;
        }

        private static decimal? SafeReadValue(Dictionary<string, decimal> dict, string key, decimal? defaultValue = null)
        {
            if(!dict.ContainsKey(key))
            {
                return defaultValue;
            }
            return dict[key];
        }
    }
}