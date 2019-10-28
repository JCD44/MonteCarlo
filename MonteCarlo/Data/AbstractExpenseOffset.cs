using System;
using System.Collections.Generic;
using System.Text;
using MonteCarlo.Calculator;
using Narvalo;

namespace MonteCarlo.Data
{
    public abstract class AbstractExpenseOffset : IExpenseOffset
    {
        public Money OffsetAmount { get; set; }

        public virtual void Offset(PortfolioWork portfolio)
        {
            portfolio.Portfolio.InvestmentAmount += OffsetAmount;
        }
        public abstract bool ShouldOffset(PortfolioWork portfolio);
    }
}
