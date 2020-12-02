using MonteCarlo.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo.Data
{
    public class OutcomeSummary
    {
        public List<Outcome> Outcomes { get; set; }
        public void ProcessOutcomes(InputData input)
        {
            Input = input;
            var worst = Outcomes.OrderBy(a => a.NewPortfolio.InvestmentAmount).ToList()[0];
            WorstAmount = worst.NewPortfolio.InvestmentAmount;
            WorstMonth = worst.PeriodNumber;
            PortolioStartTime = Outcomes[0].ReturnDataDate;
            if (WorstAmount <= 0) IsFail = true;
            var CloseAmount = Outcomes[0].PreviousPortfolio.InvestmentAmount * input.PercentageOfInitialPortolioIsClose;
            if (WorstAmount <= CloseAmount) {
                IsClose = true;
                var listOfIffyMonths = Outcomes.Where(a => a.NewPortfolio.InvestmentAmount <= CloseAmount).ToList();
                IsCloseFirstMonth = listOfIffyMonths[0].PeriodNumber;
            }

            if (WorstAmount > Input.InitialAmount)
            {
                WorstAmount = Input.InitialAmount;
                WorstMonth = 0;
            }

            var totalReturns = new Dictionary<string, Narvalo.Money>();
            foreach(var item in Outcomes)
            {
                foreach(var ret in item.Returns.Returns)
                {
                    if (totalReturns.ContainsKey(ret.Key))
                    {
                        totalReturns[ret.Key] += ret.Value;
                    } else
                    {
                        totalReturns.Add(ret.Key, ret.Value);
                    }
                }
            }
            TotalReturnsByCategory = totalReturns;

            if (IsFail)
            {
                FirstFailureMonth = Outcomes.Where(a => a.NewPortfolio.InvestmentAmount < 0).ToList()[0].PeriodNumber;
            }
            else
            {
                //Outcomes.Clear();
            }
        }

        public int RowNumber { get; set; }
        public DateTime PortolioStartTime { get; private set; }
        public int FirstFailureMonth { get; private set; }
        public int WorstMonth { get; private set; }
        public decimal WorstAmount { get; private set; }
        public bool IsFail { get; private  set; }
        public int IsCloseFirstMonth { get; private set; }
        public bool IsClose { get; private set; }
        public Dictionary<string, Narvalo.Money> TotalReturnsByCategory { get; private set; }
        public InputData Input { get; private set; }
    }
}
