using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Output
{
    public interface IWriter
    {
        void Write(string value);

    }
}
