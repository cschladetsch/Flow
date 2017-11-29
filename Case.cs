using System;

namespace Flow.Impl
{
	internal class Case<T> : ICase<T> where T : IComparable<T>
	{
		internal Case(T val, IGenerator gen)
		{
			_compare = val;
			Body = gen;
		}

		public IGenerator Body { get; private set; }

		public bool Matches(T val)
		{
			return val.CompareTo(_compare) == 0;
		}

		private readonly T _compare;
	}
}
