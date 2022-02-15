using System.Collections.Generic;

namespace ProjectGame.Utils
{
    public static class ListExtensions
    {
        public static T Remove<T>(this List<T> list, int index)
        {
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }
    }
}
