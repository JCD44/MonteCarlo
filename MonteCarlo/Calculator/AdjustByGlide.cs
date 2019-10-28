
namespace MonteCarlo.Calculator
{
    public class AdjustByGlide : IPortfolioAdjustments
    {
        private readonly string from;
        private readonly string to;
        private readonly decimal glidePerDataRow;
        public AdjustByGlide(string adjustFrom, string adjustTo, decimal glide)
        {
            from = adjustFrom;
            to = adjustTo;
            glidePerDataRow = glide;
        }
        public PortfolioWork Adjust(PortfolioWork portfolio)
        {
            var cash = portfolio.Portfolio.ComputedInvestments()[from];
            if (cash <= 0) return portfolio;//no cash to remove...
            var equity = portfolio.Portfolio.ComputedInvestments()[to];
            
            var glide = glidePerDataRow;
            //Never glide to a negative number.
            if (cash <= glide) glide = cash;
            equity = equity + glide;
            cash = cash - glide;
            portfolio.Portfolio.Investments[from] = cash;
            portfolio.Portfolio.Investments[to] = equity;

            return portfolio;
        }
        public void Init(PortfolioWork portfolio)
        {
        }
    }
}
