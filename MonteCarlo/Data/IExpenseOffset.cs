using MonteCarlo.Calculator;
using NodaMoney;

namespace MonteCarlo.Data
{
    public interface IExpenseOffset
    {
        bool ShouldOffset(PortfolioWork portfolio);
        void Offset(PortfolioWork portfolio);
        Money OffsetAmount { get; set; }

    }
}
