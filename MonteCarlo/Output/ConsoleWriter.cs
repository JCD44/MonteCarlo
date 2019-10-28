using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Output
{
    public class ConsoleWriter : IWriter
    {
        public void Write(string value)
        {
            Console.WriteLine(value);
        }
    }
}
