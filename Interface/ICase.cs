// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	public interface ICase<in T> where T : IComparable<T>
	{
		bool Matches(T val);

		IGenerator Body { get; }
	}
}
