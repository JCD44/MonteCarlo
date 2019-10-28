using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MonteCarlo.Output;

namespace MonteCarlo.Dispatch
{
    public class DispatchByArgs
    {
        public void ParseAndDispatch(string[] args)
        {
            var pArgs = GenericArgsParser.Parse(args);
            if(!string.IsNullOrEmpty(pArgs.ErrorMessage))
            {
                WriteManager.Write(pArgs.ErrorMessage);
                return;
            }
            switch(pArgs.DispatchMode)
            {
                case DispatchModes.Test:
                    new DispatchTest().Execute(pArgs);
                    break;
                case DispatchModes.Input:
                    new DispatchInputData().Execute(pArgs);
                    break;
                case DispatchModes.Help:
                    new DispatchHelp().Execute(pArgs);
                    break;
                case DispatchModes.Create:
                    new DispatchCreateFile().Execute(pArgs);
                    break;
                //TODO Help.
                default:
                    WriteManager.Write($"Invalid Mode: {pArgs.DispatchMode}");
                    return;
            }
        }
    }
}
