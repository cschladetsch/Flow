<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow
{
    /// <summary>
    /// Stores information about a time step.
    /// </summary>
    public interface ITimeFrame
    {
        DateTime Last { get; }
        DateTime Now { get; }
        TimeSpan Delta { get; }
    }
}
