using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Data
{
    public static class CsvMapper
    {

        //public static string CPI = "CPI";
        public static string Year = "Year";
        public static string Month = "Month";
        //public static string Gold = "Gold";
        //public static string Cash = "Cash";
        //public static string Equity = "SPX-TR";
        //public static string Bond = "10Y BM";
        public static string Equity_Return = "SPX-TR Return";
        public static string Bond_Return = "10YBond Return";
        public static string Cash_Return = "Cash Return";
        public static string Gold_Return = "Gold Return";
        public static string CAPE = "CAPE";
        public static Dictionary<string, string> Map = InitMap();
        private static Dictionary<string, string> DefaultNameMap = InitDefaultNameMap();



        public static string GetByDefaultName(string defaultName)
        {
            return GetByNameOf(DefaultNameMap[defaultName]);
        }

        private static string GetByNameOf(string nameOfVariable)
        {
            return Map[nameOfVariable];
        }

        private static Dictionary<string, string> InitMap()
        {
            //TODO Look up from file?
            return new Dictionary<string, string>
            {
                { nameof(Year), Year },
                { nameof(Month), Month },
                { nameof(Equity_Return), Equity_Return },
                { nameof(Bond_Return),Bond_Return},
                { nameof(Cash_Return), Cash_Return },
                { nameof(Gold_Return), Gold_Return },
                { nameof(CAPE), CAPE },

            };
        }

        private static Dictionary<string, string> InitDefaultNameMap()
        {
            //TODO Look up from file?
            return new Dictionary<string, string>
            {

                { Year, nameof(Year) },
                { Month,  nameof(Month) },
                { Equity_Return, nameof(Equity_Return) },
                { Bond_Return, nameof(Bond_Return)},
                { Cash_Return, nameof(Cash_Return) },
                { Gold_Return, nameof(Gold_Return) },
                { CAPE, nameof(CAPE) },

            };
        }

    }
}
