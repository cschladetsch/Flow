// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     A one-shot OneShotTimer that will fire its Elapsed event, and then Complete itself after a fixed time Interval.
	/// </summary>
	public interface ITimer : IPeriodic
	{
		DateTime TimeEnds { get; }
	}
}
