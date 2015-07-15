// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     A one-shot Timer that will fire its Elapsed event, and then Complete itself after a fixed time Interval.
	/// </summary>
	public interface ITimer : IPeriodic
	{
		/// <summary>
		///     Gets the game time that the timer will elapse and subsequently delete itself.
		/// </summary>
		/// <value>
		///     The soonest time that the timer will elapse
		/// </value>
		DateTime TimeEnds { get; }
	}
}
