// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	///     TODO: delta-capping, pausing, introduction of zulu/sim time differences
	/// </summary>
	internal class TimeFrame : ITimeFrame
	{
		/// <inheritdoc />
		public DateTime Last { get; internal set; }

		/// <inheritdoc />
		public DateTime Now { get; internal set; }

		/// <inheritdoc />
		public TimeSpan Delta { get; internal set; }
	}
}
