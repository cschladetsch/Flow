// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	public delegate void TimedOutHandler(ITimesOut timed);

	/// After a period of time, an instance of ITimesOut will 'time-out' if not already Completed. In this case, it will:
	public interface ITimesOut : ITransient
	{
		event TimedOutHandler TimedOut;
		ITimer Timer { get; }
		bool HasTimedOut { get; }
	}
}
