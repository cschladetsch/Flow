// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A one-shot Timer that will fire its Elapsed event then Delete itself after a fixed interval of time.
	/// </summary>
	public interface ITimer : IPeriodic
	{
		/// <summary>
		/// Gets the time that the timer will elapse and subsequently delete itself.
		/// </summary>
		/// <value>
		/// The soonest time that the timer will elapse
		/// </value>
		DateTime TimeEnds { get; }
	}
}