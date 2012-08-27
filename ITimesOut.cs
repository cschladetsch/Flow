using System;

namespace Flow
{
	/// <summary>
	/// Timed out handler.
	/// </summary>
	public delegate void TimedOutHandler(ITimesOut timed);

	/// <summary>
	/// Common interface for all instances that have a time-out
	/// </summary>
	public interface ITimesOut : ITransient
	{
		/// <summary>
		/// Occurs when this instance has timed out.
		/// </summary>
		event TimedOutHandler TimedOut;

		/// <summary>
		/// Gets the timer, if any, associated with this instance.
		/// </summary>
		/// <value>
		/// The timer assocaited with this future.
		/// </value>
		ITimer Timer { get; }

		/// <summary>
		/// Gets a value indicating whether this future has timed out.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has timed out; otherwise, <c>false</c>.
		/// </value>
		bool HasTimedOut { get; }
	}
}

