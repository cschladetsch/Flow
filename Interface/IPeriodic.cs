// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	// Periodic instances regularly fire their Elapsed event.
	// NOTE the timer will fire at most once per Kernel Step
	public interface IPeriodic : IGenerator
	{
		// The time that this periodic timer was started.
		DateTime TimeStarted { get; }

		// Gets or sets the interval. Successive Elapsed events will have not less than this TimeSpan between being fired.
		TimeSpan Interval { get; set; }

		// Periodically occurs when the timer has elapsed. Fired at most once per Kernel Step
		event TransientHandler Elapsed;
	}
}
