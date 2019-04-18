<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

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
