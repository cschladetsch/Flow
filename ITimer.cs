using System;
using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A one-shot timer that will fire its Elapsed event then delete itself after a fixed interval of time.
	/// </summary>
	public interface ITimer : IPeriodicTimer
	{
		/// <summary>
		/// Gets the time that the timer will elapse and subsequently delete itself.
		/// </summary>
		/// <value>
		/// The time ends.
		/// </value>
		DateTime TimeEnds { get; }
	}
}