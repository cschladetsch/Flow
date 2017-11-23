// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	public class FutureNotSetException : Exception
	{
		public FutureNotSetException() : base("Future value not arrived yet")
		{
		}
	}

	public class FutureAlreadySetException : Exception
	{
		public FutureAlreadySetException() : base("Future already set")
		{
		}
	}

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
