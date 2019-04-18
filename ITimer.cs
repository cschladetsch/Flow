<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow
{
    /// <summary>
    /// A one-shot OneShotTimer that will fire its Elapsed event, and then Complete itself after a fixed time Interval.
    /// </summary>
    public interface ITimer : IPeriodic
    {
        DateTime TimeEnds { get; }
    }
}
