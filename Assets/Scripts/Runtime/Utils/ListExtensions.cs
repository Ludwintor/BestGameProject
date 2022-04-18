using System.Collections.Generic;

namespace ProjectGame.Utils
{
    public static class ListExtensions
    {
        public static T Remove<T>(this IList<T> list, int index)
        {
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static void Shuffle<T>(this IList<T> list, RNG rng)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rngIndex = rng.NextInt(list.Count);
                (list[rngIndex], list[i]) = (list[i], list[rngIndex]);
            }
        }
    }
}
