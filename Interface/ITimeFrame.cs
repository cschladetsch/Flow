// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	/// Information about a time step
	/// </summary>
	public interface ITimeFrame
	{
		/// <summary>
		/// Gets the time of the last update.
		/// </summary>
		/// <value>
		/// The time at the last update
		/// </value>
		DateTime Last { get; }

		/// <summary>
		/// Gets the time at the start of this update
		/// </summary>
		/// <value>
		/// The current.
		/// </value>
		DateTime Now { get; }

		/// <summary>
		/// Gets the delta time to use for this update. Note that (Current - Last) may not always be equal to Delta
		/// </summary>
		/// <value>
		/// The delta time to use for this update.
		/// </value>
		TimeSpan Delta { get; }
	}
}