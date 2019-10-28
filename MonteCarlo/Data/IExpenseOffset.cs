using MonteCarlo.Calculator;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data
{
    public interface IExpenseOffset
    {
        bool ShouldOffset(PortfolioWork portfolio);
        void Offset(PortfolioWork portfolio);
        Narvalo.Money OffsetAmount { get; set; }

    }
}
