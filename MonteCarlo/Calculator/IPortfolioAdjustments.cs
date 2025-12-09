using MonteCarlo.Calculator;

namespace MonteCarlo.Calculator
{
    public interface IPortfolioAdjustments
    {
        public int StartMonth { get; set; }
        public int? EndMonth { get; set; }
        void Init(PortfolioWork portfolio);
        PortfolioWork Adjust(PortfolioWork portfolio);
    }
}