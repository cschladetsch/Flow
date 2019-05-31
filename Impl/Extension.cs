// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public static class Extension
    {
        public static T SetName<T>(this T self, string name) 
            where T : ITransient
        {
            self.Name = name;
            return self;
        }

        public static T AddToGroup<T>(this T self, IGroup group)
            where T : ITransient
        {
            group.Add(self);
            return self;
        }

        public static bool ContainsRef<T>(this IEnumerable<T> list, T obj)
            => list.Any(elem => ReferenceEquals(elem, obj));

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

