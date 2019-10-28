using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo
{
    public class ReturnData
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public decimal? CAPE { get; set; }
        public int? Row { get; set; }

        public Dictionary<string, decimal?> NameToPercentageReturn { get; set; } = new Dictionary<string, decimal?>();


        //,,Fractional Date, Excel Date,CPI,SPX-TR,10Y BM, SPX-TR,10Y BM, Fama-French Small Stocks,Fama-French Value Stocks,Portfolio,Cumulative Backward Return,C1,Sum of Ct,Sum of CtPt,10 Y BM 1 year avg

    }
}
