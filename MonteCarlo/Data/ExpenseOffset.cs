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

        public override bool ShouldOffset(PortfolioWork portfolio)
        {
            if (StartMonth > portfolio.MonthProcessed) return false;
            if (EndMonth != null && EndMonth < portfolio.MonthProcessed) return false;

            return true;
        }
    }
}
