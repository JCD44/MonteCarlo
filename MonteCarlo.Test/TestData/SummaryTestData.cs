using MonteCarlo.Data;
using MonteCarlo.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Test.TestData
{
    public class OutcomeSummaryTestData
    {
        public static List<OutcomeSummary> CreateData(InputData input)
        {
            var os = new OutcomeSummary
            {
                Outcomes = OutcomeTestData.CreateData(),
                RowNumber = 1,
            };
            os.ProcessOutcomes(input);

            var os1 = new OutcomeSummary
            {
                Outcomes = OutcomeTestData.CreateData(),
                RowNumber = 2,
            };
            os1.ProcessOutcomes(input);

            return new List<OutcomeSummary>
            {
                os,
                os1
            };
        } 
    }
}
