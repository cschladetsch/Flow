// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// TODO: delta-capping, pausing, introduction of zulu/sim time differences
	/// </summary>
	internal class TimeFrame : ITimeFrame
	{
		/// <inheritdoc />
		public System.DateTime Last { get; internal set; }

		/// <inheritdoc />
		public System.DateTime Now { get; internal set; }

		/// <inheritdoc />
		public System.TimeSpan Delta { get; internal set; }
	}
}