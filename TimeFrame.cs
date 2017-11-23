// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	/// <summary>
	///     TODO: delta-capping, pausing, introduction of zulu/sim time differences
	/// </summary>
	internal class TimeFrame : ITimeFrame
	{
		public DateTime Last { get; internal set; }
		public DateTime Now { get; internal set; }
		public TimeSpan Delta { get; internal set; }
	}
}
