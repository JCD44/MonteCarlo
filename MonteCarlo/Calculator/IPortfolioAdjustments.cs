using MonteCarlo.Calculator;

namespace MonteCarlo.Calculator
{
    public interface IPortfolioAdjustments
    {
        void Init(PortfolioWork portfolio);
        PortfolioWork Adjust(PortfolioWork portfolio);
    }
}