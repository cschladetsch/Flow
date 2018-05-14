// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    /// <summary>
    /// A Barrier is a Group that Completes itself when all added Transients have been Removed from it.
    /// </summary>
    public interface IBarrier : IGroup
    {
    }
}
