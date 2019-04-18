<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
    /// <summary>
    /// TODO: delta-capping, pausing, introduction of zulu/sim time differences
    /// </summary>
    internal class TimeFrame : ITimeFrame
    {
        public DateTime Last { get; internal set; }
        public DateTime Now { get; internal set; }
        public TimeSpan Delta { get; internal set; }
    }
}
