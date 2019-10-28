using KellermanSoftware.CompareNetObjects;
using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Data.JSON;
using MonteCarlo.Extensions;
using MonteCarlo.Test.TestData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Test.Data.JSON
{
    public class InputTest
    {
        public InputData CreateData()
        {
            return InputTestData.CreateData();
        }

        [Test]
        public void ConvertDataToString()
        {
            var data = CreateData();
            var output = Input.ToString(data);

            Assert.IsNotEmpty(output);
            Assert.IsTrue(output.Length > 500, "At least 500 characters of output");
            Assert.IsTrue(output.Contains(data.LastYearOfDataUsed + ""), $"Verify contains data from data.LastYearOfDataUsed");
            Assert.IsTrue(output.Contains(data.AdjustmentTranslation_AdjustByCapeRatio_Documentation.Substring(0,10) + ""), $"Verify contains data from data.AdjustmentTranslation_AdjustByCapeRatio_Documentation");
            Assert.IsTrue(output.Contains(data.AdjustmentTranslation_Documentation.Substring(0,10) + ""), $"Verify contains data from data.AdjustmentTranslation_Documentation");
            foreach(var value in data.DisplayNameToCsvHeader.Keys)
                Assert.IsTrue(output.Contains(value + ""), $"Verify contains data from data.DisplayNameToCsvHeader.Keys: ({value})");
        }

        [Test]
        public void ConvertDataBothWaysWorks()
        {
            var data = CreateData();
            var output = Input.ToString(data);
            var data2 = Input.FromString(output);

            CompareLogic compareLogic = new CompareLogic()
            {
                Config = new ComparisonConfig()
                {
                    MembersToIgnore = new List<string>()
                    {
                        "IsNormalized",
                        "IsRounded"
                    },
                    DecimalPrecision = 4,
                }
            };

            var comp = compareLogic.Compare(data, data2);

            Assert.IsTrue(comp.AreEqual, "Verify data is consistently processed. Diff: " + comp.DifferencesString);

        }

    }
}
