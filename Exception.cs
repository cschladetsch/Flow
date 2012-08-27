using System;

namespace Flow
{
	public class FutureNotSetException : System.Exception
	{
		public FutureNotSetException () : base("Future value not arrived yet")
		{
		}
	}

	public class FutureAlreadySetException : System.Exception
	{
		public FutureAlreadySetException () : base("Future already set")
		{
		}
	}
	
	public class ReentrancyException : Exception
	{
		public ReentrancyException ()  : base("Method is not re-entrant")
		{
		}
	}
}

