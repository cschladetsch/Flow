// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     Future not set exception.
	/// </summary>
	public class FutureNotSetException : Exception
	{
		public FutureNotSetException() : base("Future value not arrived yet")
		{
		}
	}

	/// <summary>
	///     Future already set exception.
	/// </summary>
	public class FutureAlreadySetException : Exception
	{
		public FutureAlreadySetException() : base("Future already set")
		{
		}
	}

	/// <summary>
	///     Re-entrancy exception.
	/// </summary>
	public class ReentrancyException : Exception
	{
		public ReentrancyException() : base("Method is not re-entrant")
		{
		}
	}

	public class EventStream<A0>
	{
		//public EventStream(Action<
	}
}
