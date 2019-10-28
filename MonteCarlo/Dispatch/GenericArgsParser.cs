using MonteCarlo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo.Dispatch
{
    public class GenericArgsParser
    {
        public string InputFilePath { get; set; }
        public string EvaluationMethod { get; set; }
        public DispatchModes DispatchMode { get; set; } = DispatchModes.Input;
        public string ErrorMessage { get; set; } = null;
        public string ReturnsInputFile { get; set; }
        public OutputMode Mode { get; set; } = OutputMode.Normal;

        public static GenericArgsParser Parse(string[] args)
        {
            var pArgs = new GenericArgsParser();
            if (GetArg("Test", args) != null)
            {
                pArgs.DispatchMode = DispatchModes.Test;
                return pArgs;
            }

            if (args == null || args.Length == 0 || GetArg("Help", args) != null || GetArg("?", args) != null)
            {
                pArgs.DispatchMode = DispatchModes.Help;
                return pArgs;
            }

            var create = GetArg("Create", args);
            if (create != null)
            {
                pArgs.InputFilePath = create;
                if (System.IO.File.Exists(create))
                {
                    pArgs.ErrorMessage += $"Unable to create file @ '{pArgs.InputFilePath}' as it exists already.{Environment.NewLine}";
                }

                pArgs.DispatchMode = DispatchModes.Create;
                return pArgs;
            }

            pArgs.EvaluationMethod = GetArg("EvalMethod", args);
            pArgs.InputFilePath = GetArg("InputFile", args);
            pArgs.ReturnsInputFile = GetArg("ReturnsInputFile", args);
            var outputMode = GetArg("Output", args);

            if (Enum.TryParse(outputMode, out OutputMode mode))
            {
                pArgs.Mode = mode;
            }

            if (!System.IO.File.Exists(pArgs.InputFilePath))
            {
                pArgs.ErrorMessage += $"Unable to find input file @ '{pArgs.InputFilePath}'.{Environment.NewLine}";
            }

            if (!System.IO.File.Exists(pArgs.ReturnsInputFile))
            {
                pArgs.ErrorMessage += $"Unable to find returns file @ '{pArgs.ReturnsInputFile}'.{Environment.NewLine}";
            }

            return pArgs;
        }

        private static string GetArg(string val, string[] args)
        {
            if (val == null) return null;
            if (args == null || args.Length == 0) return null;
            //Handles variants of args.
            char[] delims = { '=', ':', ' ' };
            foreach(var item in args)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                //Exact Match
                var newItem = item;
                //Remove all minuses
                while (newItem.StartsWith("-")) newItem = newItem.Substring(1);

                if(string.Equals(val, newItem, StringComparison.InvariantCultureIgnoreCase))
                {
                    return val;
                }
                foreach (var delim in delims)
                {
                    if (item.Contains(delim))
                    {
                        var split = newItem.Split(delim, 2);
                        if (string.Equals(val, split[0], StringComparison.InvariantCultureIgnoreCase))
                        {
                            return split[1];
                        }
                    }
                }
            }

            return null;

        }
    }
}
