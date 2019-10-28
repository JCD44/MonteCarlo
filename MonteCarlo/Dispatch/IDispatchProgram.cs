using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Dispatch
{
    public interface IDispatchProgram
    {
        void Execute(GenericArgsParser args);
    }
}
