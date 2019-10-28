using CsvHelper;
using MonteCarlo.Data;
using MonteCarlo.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MonteCarlo
{
    public class CsvParser
    {

        private void ValidateFile(string file, InputData input)
        {
            var header = System.IO.File.ReadLines(file).First();
            //Validate file is good by validating header record.
            var listOfRequiredWords = new List<string>()
            {
                CsvMapper.GetByDefaultName(CsvMapper.Year),
                CsvMapper.GetByDefaultName(CsvMapper.Month),
                CsvMapper.GetByDefaultName(CsvMapper.CAPE),
            };

            listOfRequiredWords.AddRange(input.DisplayNameToCsvHeader.Values);
            foreach(var item in listOfRequiredWords)
            {
                if(!header.Contains(item))
                {
                    throw new InvalidDataException($"Header of CSV (the first line of the file) does not contain '{item}'.  Header: '{header}'");
                }
            }
        }

        public IEnumerable<ReturnData> GetData(InputData input)
        {
            var file = input.FilePath;
            ValidateFile(file, input);
            
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader))
            {
                var records = new List<ReturnData>();
                csv.Read();
                csv.ReadHeader();
                int row = 0;
                while (csv.Read())
                {
                    var record = new ReturnData
                    {
                        Year = csv.Get<int?>(CsvMapper.GetByDefaultName(CsvMapper.Year)),
                        Month = csv.Get<int?>(CsvMapper.GetByDefaultName(CsvMapper.Month)),
                        Row = row++,
                        NameToPercentageReturn = new Dictionary<string, decimal?>(),
                        CAPE = csv.Get(CsvMapper.GetByDefaultName(CsvMapper.CAPE)).ToNumber()
                    };

                    foreach(var item in input.DisplayNameToCsvHeader)
                    {
                        record.NameToPercentageReturn.Add(item.Key, csv.Get(item.Value).ToNumber());
                    }

                    records.Add(record);
                }

                return records;
            }
        }

    }
}
