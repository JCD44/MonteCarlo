using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Output
{
    public class WriteManager
    {
        public static IWriter Writer { get; set; } = new ConsoleWriter();

        public static void Write(string value)
        {
            Writer.Write(value);
        }

    }
}
