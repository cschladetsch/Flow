// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	///     Timed out handler.
	/// </summary>
	public delegate void TimedOutHandler(ITimesOut timed);

	/// <summary>
	///     After a period of time, an instance of ITimesOut will 'time-out' if not already Completed. In this case, it will:
	///     <list>
	///         <item>fire its TimedOut event</item>
	///         <item>set its HasTimedOut property to true, and then</item>
	///         <item>Complete itself</item>
	///     </list>
	/// </summary>
	public interface ITimesOut : ITransient
	{
		/// <summary>
		///     Gets the timer, if any, associated with this instance.
		/// </summary>
		/// <value>
		///     The timer associated with this future.
		/// </value>
		ITimer Timer { get; }

		/// <summary>
		///     Gets a value indicating whether this future has timed out.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance has timed out; otherwise, <c>false</c>.
		/// </value>
		bool HasTimedOut { get; }

		/// <summary>
		///     Occurs when this instance has timed out.
		/// </summary>
		event TimedOutHandler TimedOut;
	}
}
