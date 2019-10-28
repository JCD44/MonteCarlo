using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Output
{
    public class StringWriter : IWriter
    {
        private readonly StringBuilder sb = new StringBuilder();

        public void Write(string value)
        {
            sb.AppendLine(value);
        }

        public string Output() { return sb.ToString(); }
    }
}
