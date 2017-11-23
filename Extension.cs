// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

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

#if NOT_NEEDED_ANYMORE
	public delegate RT Func<RT>();

	public delegate RT Func<T0, RT>(T0 t0);

	public delegate RT Func<T0, T1, RT>(T0 t0, T1 t1);

	public delegate RT Func<T0, T1, T2, RT>(T0 t0, T1 t1, T2 t2);

	public delegate RT Func<T0, T1, T2, T3, RT>(T0 t0, T1 t1, T2 t2, T3 t3);

	public delegate RT Func<T0, T1, T2, T3, T4, RT>(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4);

	public delegate void Action<T0>(T0 t0);

	public delegate void Action<T0, T1>(T0 t0, T1 t1);
#endif
}
