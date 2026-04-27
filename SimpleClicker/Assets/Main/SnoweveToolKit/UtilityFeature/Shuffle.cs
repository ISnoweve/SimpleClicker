using System;
using System.Collections.Generic;
using System.Linq;

namespace _Main.UtilityFeature
{
    public static class Shuffle
    {
        static readonly Random Generator = new();

        public static void ShuffleList<T>(this IList<T> list, Random rng = null, int take = 0)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (take == 0) take = 3;

            var rnd = rng ?? Generator;
            for (int i = list.Count - 1; i > take; i--)
            {
                int swapIndex = rnd.Next(0, i + 1); 
                T tmp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = tmp;
            }
        }

        public static IEnumerable<T> ShuffleIEnumerable<T>(this IEnumerable<T> sequence, int take = 0)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (take == 0) take = 3;

            if (sequence is IList<T> list)
            {
                list.ShuffleList(Generator, take);
                return list;
            }

            var retArray = sequence.ToArray();
            for (var i = retArray.Length - 1; i > take; i--)
            {
                var swapIndex = Generator.Next(0, i + 1);
                var temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }

            return retArray;
        }
    }
}