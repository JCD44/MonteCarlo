using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class MathExtensions
    {
        //from https://stackoverflow.com/questions/8137391/percentile-calculation/
        //public static double ToPercentile(this double[] sequence,  double percentile)
        //{
        //    Array.Sort(sequence);
        //    int N = sequence.Length;
        //    double n = (N - 1) * percentile + 1;
        //    // Another method: double n = (N + 1) * excelPercentile;
        //    if (n == 1d) return sequence[0];
        //    else if (n == N) return sequence[N - 1];
        //    else
        //    {
        //        int k = (int)n;
        //        double d = n - k;
        //        return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
        //    }
        //}

        //public static double ToPercentile(this List<double> sequence, double percentile)
        //{
        //    return ToPercentile(sequence.ToArray(), percentile);
        //}

        public static decimal ToPercentile(IEnumerable<decimal> seq, double percentile)
        {
            var elements = seq.ToArray();
            Array.Sort(elements);
            decimal realIndex = (decimal)(percentile * (elements.Length - 1));
            int index = (int)realIndex;
            decimal frac = realIndex - index;
            if (index + 1 < elements.Length)
                return elements[index] * (1 - frac) + elements[index + 1] * frac;
            else
                return elements[index];
        }

        public static decimal ToPercentile(this List<decimal> sequence, double percentile)
        {
            return ToPercentile(sequence.ToArray(), percentile);
        }
    }
}
