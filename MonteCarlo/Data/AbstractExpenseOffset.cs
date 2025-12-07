using MonteCarlo.Calculator;
using NodaMoney;

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
