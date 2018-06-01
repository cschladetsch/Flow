// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections.Generic;

namespace Flow
{
    /// <summary>
    /// A Barrier is a Group that Completes itself when all added Transients have been Removed from it.
    /// </summary>
    public interface IBarrier : IGroup
    {
        IBarrier ForEach<T>(Action<T> act) where T : class;
    }
}
