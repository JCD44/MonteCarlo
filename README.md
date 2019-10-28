# MonteCarlo

A package designed to allow market investment simulations with varying inputs and style of market analysis.  You control what data is used by pointing to a CSV, how the investment simulation should behave, such as how your portfolio is setup and how it changes over time.  Then you select the mode, either Monte Carlo or simulating actual history by going through all years.  Finally you run the program and see the results.


## Getting Started (With the exe)

1. You select a data set of investment returns data which is yearly or monthly and the application will take that data.  I suggest using: https://earlyretirementnow.com/2017/01/25/the-ultimate-guide-to-safe-withdrawal-rates-part-7-toolbox/
   - The doc linked above needs to be exported by saving as CSV.  There are several steps needed for that.  See details on that below in "Export CSV" section.
2. You create an input file.  This is a complex file, so the application will create the file for you.  To do that use the following command MonteCarlo.exe -Create:FilePath.json
3. Edit the JSON file.  If using the file suggested above, the only changes required are in the "Export CSV" section.  For more help see "Input File".
4. Now with a good input file and a data file, simply run the program.  See Command line options below for details on how to run the program.

### Command line options

Supported Args:
* Help - Displays this help prompt
* EvalMethod - Provides ability to evaluate performance by different means.  Supported methods: 
  - AllYears
  - RandomOrder
* InputFile - The JSON input file that defines the scenario.
* ReturnsInputFile - The file used to define the returns.  Overrides input file value.
* Create - Creates a sample JSON input file.
* Output - The output generated by the program.  Supported modes: Normal, Files, Verbose, GiantCsvConsole, GiantCsvFile.  
  - Files - Outputs CSVs for each simulation.  
  - Verbose - Gives details of each simulation
  - GiantCsvConsole - Outputs "Files" like data in a single CSV format to the command line.
  - GiantCsvFile - Outputs "Files" like data in a single CSV format to single file.
  - Normal - Needs not be specified ever.  
  
Supported Method of passing in args:
* program.exe -Arg
* program.exe Arg
* program.exe "Arg"


Not Supported:
* program.exe -"Arg"

Program Usage Examples:
* program.exe InputFile="C:\fi\le.json" EvalMethod="RandomOrder"
* program.exe -InputFile="C:\fi\le.json"
* program.exe -Create:"C:\valid\file\path\newFile.json"
* program.exe --help
* program.exe InputFile="C:\fi\le.json" EvalMethod="AllYears" -Output:GiantCsvConsole

### Input File

The input file allows for a lot of complexity.  In general it allows you to:
* Simulate income or expense steams between two periods.
* Change Holdings by time frame, CAPE
* Maintain Min/Max holdings.

The file comes with some documentation, but I'll cover the elements not auto-documented:

* DisplayNameToCsvHeader: A set of common names you will use throughout the input file to the names in the CSV file.  For example, the CSV might have the header "S&P 500" while you might refer to it as "Equity".  In the JSON it would appear like this: "DisplayNameToCsvHeader": { "Equity": "S&P 500", }
* Portfolio: Set of Investments and how they are allocated.  These values should add up to 100.  The names should be your display names, not the CSV header.
* InitialAmount: Initial amount invested.
* NumberOfSimulations: Number of attempts, when using RandomOrder.  To give you an idea of the processing, 1000 simulations with 600 periods will give you 600,000 resulting periods.
* PeriodsWithinSimulation: How many periods/months the simulation for run for.  If you started a simulation January, set RowsOfReturns to 12 and we assuming monthly results, the simulation will run for 1 year.  Set RowsOfReturns to 600 will run 50 years. 
* StartMonth: The first month to execute when using AllYears.  If set, StartYear will be set too.
* StartYear: The first year to execute  when using AllYears.  If StartMonth is not set, it will run on the first month.
* LastYearOfDataUsed: The last year of data to be used when using AllYears.  Useful if you want to simulate a particular era.
* AdjustmentTranslation: This injects all the adjustments you want to make.  All the various adjustments can be seen in the example file.  More detail below.
* IncomeAndExpenses: Defines Start and End "Month" (The number of periods or rows that are executed) and amount to adjust by.  Imagine you are making a monthly income of 500 from a part time job you plan on staying at for the next 3 years.  So month 1-36 would be set to an amount of 500.


### Export CSV

(Last tested 10/2019)

1. Go to https://earlyretirementnow.com/2017/01/25/the-ultimate-guide-to-safe-withdrawal-rates-part-7-toolbox/ and find the google doc.
2. Save copy locally.
3. Go to the Stock/Bond Returns tab.
4. Select the second set of columns: SPX-TR, 10Y BM, Cash, Custom, Gold (Currently columns L-O).
5. Click Increase Decimal Place 14 times.
6. Delete all rows above the header row (Currently rows 1-4).
7. Export to CSV by pressing File->Download->Comma-Separated File.  Save file.
8. Open CSV file in excel-like program (it can be google docs, I used LibreOffice 5).
9. Delete first set of columns: SPX-TR, 10Y BM, Cash, Custom, Gold (Currently columns F-J).
10. Save.

```
Make sure your input file copies the column names exactly.  e.g.:
"DisplayNameToCsvHeader": {
    "Equity": "SPX-TR",
    "Bond": "10Y BM",
    "Cash": "Cash",
    "Gold": "Gold"
  }
```

## Understanding the Modes and Outputs

### All Years:

All Years mode runs from year x to y, based upon filtering.  If the number of periods extends beyond the data, it will randomly pick all future dates.  If you desire a different behaviour, simply supply data beyond the current time and it will be used.


Here is an example running from 1972 to 1975 for 50 years.  That is 4x12 simulations or 48 simulations.  In this case, no failures occurred, nothing ever dropped below 20% of the initial portfolio, so nothing was reported.  All statistics are based solely within the context of the analysis run.
```
 * Simulations: 48
 * Failures: 0
 * Close to Failures (20.0% or less initial portfolio): 0
 * Failure Probability: 0.00%
 * Failure Details:
  -
 * Close Details:
  -
 * Top 10 Worst Details:
  -  Initial Date: 7/1/1973, Worst Month: 109, Worst Amount: 560,799.21
  -  Initial Date: 6/1/1973, Worst Month: 110, Worst Amount: 565,859.14
...Trimmed
```

Here is yet another example, run from 1960 to 1968.  This shows failures and how many portfolios with the input file given.  Obviously these are all date based.  These reports give you a feel for how bad things could get, but don't output any details.
```
 * Simulations: 108
 * Failures: 13
 * Close to Failures (20.0% or less initial portfolio): 27
 * Failure Probability: 12.04%
 * Failure Details:
  -  Initial Date: 1/1/1960, First Close Month: 510, First Failure Month: 598, Worst Month: 600, Worst Amount: -7,581.40
  -  Initial Date: 12/1/1960, First Close Month: 499, First Failure Month: 582, Worst Month: 600, Worst Amount: -48,769.69
  -  Initial Date: 1/1/1961, First Close Month: 497, First Failure Month: 581, Worst Month: 600, Worst Amount: -55,392.34
...Trimmed
 * Close Details:
  -  Initial Date: 2/1/1960, First Close Month: 584, Worst Month: 589, Worst Amount: 141,977.22
  -  Initial Date: 3/1/1960, First Close Month: 516, Worst Month: 600, Worst Amount: 31,246.53
  -  Initial Date: 5/1/1960, First Close Month: 582, Worst Month: 586, Worst Amount: 149,269.62
  ...Trimmed
 * Top 10 Worst Details:
  -  Initial Date: 2/1/1961, Worst Month: 600, Worst Amount: -194,657.45
  -  Initial Date: 4/1/1961, Worst Month: 600, Worst Amount: -146,693.01
  -  Initial Date: 3/1/1961, Worst Month: 600, Worst Amount: -130,889.95
...Trimmed
```

### Random Order:

This is 100 simulations executed and 7 of them failed and another 2 came close to failure.  The "initial date" might be confusing, but what it tells you is the earliest randomly selected date in your portfolio that was picked.
```
 * Simulations: 100
 * Failures: 7
 * Close to Failures (20.0% or less initial portfolio): 2
 * Failure Probability: 7.00%
 * Failure Details:
  -  Initial Date: 12/1/1889, First Close Month: 264, First Failure Month: 328, Worst Month: 586, Worst Amount: -1,346,889.14
  -  Initial Date: 3/1/1947, First Close Month: 311, First Failure Month: 368, Worst Month: 592, Worst Amount: -1,168,352.41
  ...Trimmed
 * Close Details:
  -  Initial Date: 7/1/1882, First Close Month: 564, Worst Month: 594, Worst Amount: 148,253.31
  -  Initial Date: 10/1/1920, First Close Month: 598, Worst Month: 600, Worst Amount: 198,446.17
 * Top 10 Worst Details:
  -  Initial Date: 12/1/1889, Worst Month: 586, Worst Amount: -1,346,889.14
  -  Initial Date: 3/1/1947, Worst Month: 592, Worst Amount: -1,168,352.41
  ...Trimmed
```


All reporting assumes a monthly basis, if you are using yearly returns, then simply interpret "Month" as period.

### Report Mode Verbose

Verbose provides a human readable summary to give you an idea of the results, but is slightly less detailed than the CSV modes.

```
***********Run # 99***********
-----
Started: 1/1/1894
- Is Failure: False
- Is Close: False
- Worst Month: 1
- Worst Amount: 1,021,919.87
- Total Returns By Category: Equity='3,895,807.85'; Bond='100,317.80'; Cash='1,870,095.86'; Gold='56,582.54';
-----
Period: 1 - 1/1/1894 - 1,000,000.00 changed to 1,021,919.87 - Period P/L: 25,503.21 USD - Returns P/L: Equity='2,200.29'; Bond='904.83'; Cash='18,265.39'; Gold='4,132.70';
Period: 2 - 8/1/1871 - 1,021,919.87 changed to 1,041,465.96 - Period P/L: 23,129.42 USD - Returns P/L: Equity='6,504.62'; Bond='1,202.29'; Cash='13,001.02'; Gold='2,421.49';
Period: 3 - 4/1/1953 - 1,041,465.96 changed to 1,033,989.33 - Period P/L: -3,893.30 USD - Returns P/L: Equity='-4,741.34'; Bond='-187.78'; Cash='1,035.82'; Gold='0.00';
Period: 4 - 8/1/1922 - 1,033,989.33 changed to 1,062,097.86 - Period P/L: 31,691.86 USD - Returns P/L: Equity='19,283.78'; Bond='834.71'; Cash='9,745.67'; Gold='1,827.69';
...Trimmed
```

### Report Mode GiantCsv*, Files

The Files mode creates a file for every single simulation, which could be thousands.  You will get an output like this:

```
Writing data to C:\Users\JCD\AppData\Local\Temp\Execution_98_10a43f27-afad-4e44-8e5d-8d07bf6eee17.csv
Writing data to C:\Users\JCD\AppData\Local\Temp\Execution_74_ef8cd8fb-541b-4060-a844-e5e101fab507.csv
Writing data to C:\Users\JCD\AppData\Local\Temp\Execution_99_c7d89beb-b94d-4281-bef6-30ebc915d877.csv
```

Here is what each file will look like:

```
"Month", "Year","Period Number","% in Equity","% in Bond","% in Cash","% in Gold","P/L in Equity","P/L in Bond","P/L in Cash","P/L in Gold","P/L Total","Returns",
"1", "1975", "1","20.28","4.93","59.86","14.93","24,841.78","-330.31","1,250.61","-9,463.06","16,299.01","1,012,715.68",
"2", "1975", "2","20.56","4.86","59.72","14.86","11,565.34","167.73","-1,471.81","3,501.59","13,762.85","1,022,895.19",
```

NOTE: GiantCsvConsole will NOT create a file, only outputting the data to the command line, while GiantCsvFile will output a single file, just like the "Files" method.


## The Code

The code can give you insights into how the program works.  In particular not every value is outputted and sometimes you want to get a deeper look into what the application is doing.  Fortunately for you, it is all open source and should be as easy as opening the project in visual studio.

### Prerequisites

There are several nuget packages this depends on:
* Narvalo Money
* CsvHelper
* CloneExtensions


### Installing

1. Download the source.
2. Open the project with VS 2019.  Community ed. will work fine.
3. Right click on the project MonteCarlo and select Publish
4. Select win-x64 or your platform. Click publish.


## Running the tests

The tests should mostly just pass, however, the integration tests require that a specific CSV be placed in a specific directory.  To create the CSV, follow the steps above in Export CSV.  

### Break down the testing

Given that these numbers need to be right if the project will make any sense at all, I tried very hard to make sure my numbers matched big ERN's spreadsheet.  I have several integration tests designed around this requirement.

I know there isn't as many tests as I'd like, but this project was originally built just for me, without any plans to open source the code or do anything besides hand test/integration test it as I built it.  As that has changed, I've tried to add a fair bit of unit testing coverage but more could be done.  

### Big Features I'd like to have

* Have the CAPE response support multiple 'CAPE'.  e.g. Historic bond yields or CAPE for a specific set of investments.
* Better error handling on missing columns.
* Having CAPE response support multiple different evaluations, e.g. <, >, >=, <=,


## Built With

* Visual Studio 2019
* .net core 2.2

## Contributing

Feel free to contact me, however any pull request without clear documentation of intent, including tests, will be rejected.

## Versioning

I only plan on versioning the windows build that I provide links to.  For now there are now windows builds, you must download the source, so no version numbers exist.


## Primary Authors

* **JCD** - *Initial work*


## License

This project is licensed under the GPL v2 License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Tyler9000 for giving me the idea in the first place.
* Big ERN's data got me started.
* ERE for giving me motivation to produce this app.


### Fine Print
Nothing this application is or outputs can be considered advice or guidance for investment decisions or, for that matter, any other decisions. 
If uncertain, always consult an investment advisor and/or accountant.

JCD makes no representations as to accuracy, completeness, currentness, suitability, or validity of any information generated by this application and will not be liable for any errors, omissions, or delays in this information or any losses, injuries, or damages arising from its display or use. Furthermore, JCD assumes no responsibility for the accuracy, completeness, currentness, suitability, and validity of any and all external links you find here. JCD assumes no responsibility or liability for postings and pull requests by users.
