using System;
using System.Collections.Generic;

namespace Flow
{
	public static class Extension
	{
		public static bool ContainsRef<T>(this IEnumerable<T> list, T obj)
		{
			foreach (var elem in list)
			{
				if (object.ReferenceEquals(elem, obj))
				{
					return true;
				}
			}
			return false;
		}

		public static void RemoveRef<T>(this IList<T> list, T obj)
		{
			for (var n = 0; n < list.Count; ++n)
			{
				if (object.ReferenceEquals(list[n], obj))
				{
					list.RemoveAt(n);
					return;
				}
			}
		}
	}

	public delegate RT Func<RT>();

	public delegate RT Func<T0, RT>(T0 t0);

	public delegate void Action<T0>(T0 t0);
}

