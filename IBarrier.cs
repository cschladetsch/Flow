// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    /// <inheritdoc />
    /// <summary>
    /// A Barrier is a Group that Completes itself when all added Transients have been Removed from it.
    /// </summary>
    public interface IBarrier : IGroup
    {
    }
}
