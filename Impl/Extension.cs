// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public static class Extension
    {
        public static bool ContainsRef<T>(this IEnumerable<T> list, T obj)
        {
            return list.Any(elem => ReferenceEquals(elem, obj));
        }

        public static void RemoveRef<T>(this IList<T> list, T obj)
        {
            for (var n = 0; n < list.Count; ++n)
            {
                if (!ReferenceEquals(list[n], obj))
                    continue;

                list.RemoveAt(n);
                return;
            }
        }
    }
}
