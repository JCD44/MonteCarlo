using CloneExtensions;
using MonteCarlo.Calculator;
using MonteCarlo.Extensions;
using MonteCarlo.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonteCarlo.Data.JSON
{
    public static class Input
    {
        public static string ToFile(InputData input, string path)
        {

            File.WriteAllText(path, ToString(input));

            return path;
        }

        public static string ToString(InputData input)
        {
            var tmpInput = input.GetClone();
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(Narvalo.Currency), "MinorCurrency");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Currency), "One");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsNormalized");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsRoundable");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsRounded");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsZero");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsNegative");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsNegativeOrZero");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsPositive");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsPositiveOrZero");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "Sign");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "Currency");
            jsonResolver.IgnoreProperty(typeof(InputData), "Adjustments");
            jsonResolver.IgnoreProperty(typeof(InputData), "InitialAmt");
            jsonResolver.IgnoreProperty(typeof(Portfolio), "TemporaryInvestmentAdjustments");
            jsonResolver.IgnoreProperty(typeof(Portfolio), "InvestmentAmount");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = jsonResolver
            };

            tmpInput.Portfolio.InvestmentAmount = 0.AsMoney();

            return
                JsonConvert.SerializeObject(tmpInput, Formatting.Indented, settings);
        }

        public static InputData FromFileSafe(string path, InputData defaultValue)
        {


            InputData data;
            if (!File.Exists(path))
            {
                WriteManager.Write($"Unable to find '{path}'.  Using default values.");
                data = defaultValue;
            }
            else
            {
                data = FromString(File.ReadAllText(path));
            }

            return data;
        }

        public static InputData FromString(string value)
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(Narvalo.Currency), "MinorCurrency");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Currency), "One");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsNormalized");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsRoundable");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsRounded");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsZero");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsNegative");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsNegativeOrZero");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsPositive");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "IsPositiveOrZero");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "Sign");
            jsonResolver.IgnoreProperty(typeof(Narvalo.Money), "Currency");


            var settings = new JsonSerializerSettings
            {
                ContractResolver = jsonResolver
            };

            settings.Converters.Add(new DeserializeMoney());

            var data = JsonConvert.DeserializeObject<InputData>(value, settings);

            data.Portfolio.InvestmentAmount = data.InitialAmount.AsMoney();

            return data;
        }
    }
}
