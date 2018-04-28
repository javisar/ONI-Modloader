using System.Collections.Generic;

namespace MaterialColor.Extensions
{
    public static class Class1
    {
        public static bool NullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}