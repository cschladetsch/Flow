// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     Stores information about a time step.
	/// </summary>
	public interface ITimeFrame
	{
		/// <summary>
		///     Gets the canonical time of the last update.
		/// </summary>
		/// <value>
		///     The canonical time that the last update was executed at.
		/// </value>
		DateTime Last { get; }

		/// <summary>
		///     Gets the canonical time of the current update. This does not change as the update is being performed.
		/// </summary>
		/// <value>
		///     The canonical time of the current update. This does not change as the update is being performed.
		/// </value>
		DateTime Now { get; }

		/// <summary>
		///     Gets the delta time to use for this update. Note that (Now - Last) may not always be equal to Delta
		/// </summary>
		/// <value>
		///     The delta time to use for this update.
		/// </value>
		TimeSpan Delta { get; }
	}
}
