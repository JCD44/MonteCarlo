using MonteCarlo.Data;
using MonteCarlo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Calculator
{
    public class AdjustByCapeRatio : IPortfolioAdjustments
    {
        private readonly string from;
        private readonly string to;
        private readonly decimal maxCapeBeforeAdjustment;
        private readonly decimal percentageAdjusted;
        private readonly decimal additionalAdjustmentPerCapeExcess;
        public AdjustByCapeRatio(string adjustFrom,  string adjustTo, 
            decimal? maxCapeBeforeAction, decimal? percentageAdjusted, decimal? additionalAdjustmentPerCapeExcess)
        {
            from = adjustFrom;
            to = adjustTo;
            if (maxCapeBeforeAction.HasValue)
                this.maxCapeBeforeAdjustment = maxCapeBeforeAction.Value;
            else 
                maxCapeBeforeAdjustment = 20m;

            if (percentageAdjusted.HasValue)
                this.percentageAdjusted = percentageAdjusted.Value;
            else
                this.percentageAdjusted = 20M;

            if (additionalAdjustmentPerCapeExcess.HasValue)
                this.additionalAdjustmentPerCapeExcess = additionalAdjustmentPerCapeExcess.Value;
            else
                this.additionalAdjustmentPerCapeExcess = 10m;
        }

        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            var maxCapeToDoNothing = maxCapeBeforeAdjustment;
            var percentageAdjustment = percentageAdjusted;
            var amountOfAdditionalCapePerAdjustment = additionalAdjustmentPerCapeExcess;
            var cape = portfolio.ReturnData.NameToCape[from];
            if (cape == null || !cape.HasValue) return portfolio;
            if (cape <= maxCapeToDoNothing) return portfolio;

            var equity = portfolio.Portfolio.ComputedInvestments()[from];
            if (equity <= 0) return portfolio;//no equity to remove...
            var capeOverage = cape - maxCapeToDoNothing;
            var actualAdjustment = (decimal) (capeOverage / amountOfAdditionalCapePerAdjustment) * percentageAdjustment;
            if (equity < actualAdjustment) actualAdjustment = equity;

            portfolio.Portfolio.TemporaryInvestmentAdjustments.Upsert(from, -1 *  actualAdjustment);
            portfolio.Portfolio.TemporaryInvestmentAdjustments.Upsert(to, actualAdjustment);

            return portfolio;
        }

        public void Init(PortfolioWork portfolio)
        {
            if(!portfolio.ReturnData.NameToCape.ContainsKey(from))
            {
                throw new Exception($"Unable to find '{from}' in DisplayNameOfCapeMeasureToHeader.  Given current configuration would allow from to be: '{string.Join("','", portfolio.ReturnData.NameToCape.Keys)}'.");
            }
        }
    }
}