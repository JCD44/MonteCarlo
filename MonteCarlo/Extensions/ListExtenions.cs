using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class ListExtenions
    {
        //From https://stackoverflow.com/questions/1287567/is-using-random-and-orderby-a-good-shuffle-algorithm/1287572
        private static Random random = new Random();
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                int swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

            public static T PickRandom<T>(this IEnumerable<T> source)
            {
                return source.PickRandom(1).Single();
            }

            public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
            {
                return source.Shuffle().Take(count);
            }


    }
}
