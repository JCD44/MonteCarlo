using MonteCarlo.Data;
using MonteCarlo.Extensions;
using MonteCarlo.Output;
using MonteCarlo.Output.Report;
using MonteCarlo.Test.TestData;
using NUnit.Framework;
using System.Collections.Generic;

namespace MonteCarlo.Test.Output.Report
{
    public class ReportsTest
    {

        private static InputData Input { get; set; } = InputTestData.CreateData();
        private List<OutcomeSummary> Summary { get; set; } = OutcomeSummaryTestData.CreateData(Input);


        [Test]
        public void VerboseReport_OutputSuccessful()
        {
            var str = CreateTest(new VerboseReport());

            Assert.NotNull(str);
            foreach (var item in Summary)
            {
                Assert.IsTrue(str.Contains($"***********End of Run # {item.RowNumber}***********"));
                Assert.IsTrue(str.Contains(item.TotalReturnsByCategory["Bond"].MakeReadable()));
            }
            Assert.IsTrue(str.Length > 100);
        }

        [Test]
        public void EmptyReport_OutputSuccessful()
        {
            var str = CreateTest(new EmptyReport());

            Assert.NotNull(str);
            Assert.IsTrue(str.Length <= 0);

        }

        [Test]//Not purely a unit test, but good enough for me.
        public void GiantCsvFileReport_OutputSuccessful()
        {
            var str = CreateTest(new GiantCsvFileReport());

            Assert.NotNull(str);
            Assert.IsTrue(str.Length > 10);
            var file = str.Substring(str.IndexOf("to ") + 3);
            file = file.TrimEnd();
            Assert.IsTrue(System.IO.File.Exists(file));
            System.IO.File.Delete(file);
        }

        [Test]
        public void CsvFilesReport_OutputSuccessful()
        {
            var str = CreateTest(new CsvFilesReport());

            Assert.NotNull(str);
            Assert.IsTrue(str.Length > 100);

            var fileSplit = "to ";
            foreach (var item in str.Split('\n'))
            {
                if (item?.Contains(fileSplit) == false) continue;
                var file = item.Substring(item.IndexOf(fileSplit) + 3);
                file = file.TrimEnd();
                Assert.IsTrue(System.IO.File.Exists(file));
                System.IO.File.Delete(file);
            }

        }

        [Test]
        public void GiantCsvConsoleReport_OutputSuccessful()
        {
            var r = new GiantCsvConsoleReport
            {
                WriterToResetTo = new StringWriter()
            };

            CreateTest(r);
            var str = ((StringWriter)r.WriterToResetTo).Output();
            Assert.NotNull(str);
            Assert.IsTrue(str.Length > 100);
        }

        public string CreateTest(IReport report)
        {
            var sw = new StringWriter();
            WriteManager.Writer = sw;

            report.Report(Summary, Input);

            return sw.Output();
        }

    }
}
