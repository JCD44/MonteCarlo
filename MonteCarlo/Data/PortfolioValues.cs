using System.Collections.Generic;

namespace MonteCarlo.Data
{
    public class PortfolioValues
    {
        public Dictionary<string, decimal> Investments { get; set; } = new Dictionary<string, decimal>();
    }
}
