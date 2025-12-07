using MonteCarlo.Calculator;

namespace MonteCarlo.Data
{
    public class ExpenseOffset : AbstractExpenseOffset
    {
        public ExpenseOffset()
        {
        }

        public int StartMonth { get; set; } = 0;
        public int? EndMonth { get; set; } = null;
        //This is meant to support expenses which stop not by time but by percentage of initial portfolio.
        public int? MinPercentageFromInitialPortfolio { get; set; } = null;

        public override bool ShouldOffset(PortfolioWork portfolio)
        {
            if (StartMonth > portfolio.MonthProcessed) return false;
            if (EndMonth != null && EndMonth < portfolio.MonthProcessed) return false;
            if (MinPercentageFromInitialPortfolio != null)
            {
                //100 dollars / 100 = 1 dollar * 95 = 95 dollar minimum
                var minimumInDollars = ((portfolio.Input.InitialAmount / 100) * MinPercentageFromInitialPortfolio);
                if (portfolio.Portfolio.InvestmentAmount.ToDecimal() < minimumInDollars) return false;
            }

            return true;
        }
    }
}
