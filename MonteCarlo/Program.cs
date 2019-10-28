using CloneExtensions;
using MonteCarlo.Calculator;
using MonteCarlo.Data;
using MonteCarlo.Dispatch;
using MonteCarlo.Extensions;
using MonteCarlo.Output;
using MonteCarlo.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo
{
    class Program
    {
        static void Main(string[] args)
        {
            new DispatchByArgs().ParseAndDispatch(args);
        }

    }
}
