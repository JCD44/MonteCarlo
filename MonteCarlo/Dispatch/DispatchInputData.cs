using MonteCarlo.Data;
using MonteCarlo.Data.JSON;
using MonteCarlo.Output;
using MonteCarlo.Simulations;
using System.Collections.Generic;
using System.IO;

namespace MonteCarlo.Dispatch
{
    public class DispatchInputData : IDispatchProgram
    {

        protected static readonly Dictionary<string, ISimulation> Simulation = new Dictionary<string, ISimulation>()
        {
            { "AllYears", new AllYearsSimulation() },
            { "RandomOrder", new RandomOrderSimulation() }
        };

        public void Execute(GenericArgsParser args)
        {
            var input = ArgsToInputData(args);

            var method = args.EvaluationMethod;
            var defaultMethod = "AllYears";
            if (method==null || !Simulation.ContainsKey(method))
            {
                WriteManager.Write($"Invalid evaluation method '{method}'.  Replacing with '{defaultMethod}'");
                method = defaultMethod;    
            }
            Simulation[method].Mode = args.Mode;
            Simulation[method].ExecuteSimulate(input);

        }

        protected InputData ArgsToInputData(GenericArgsParser args)
        {
            var path = args.InputFilePath;

            var data = Input.FromFileSafe(path, new InputData());

            if (File.Exists(args.ReturnsInputFile))
            {
                data.FilePath = args.ReturnsInputFile;
            }

            return data;
        }
    }
}
