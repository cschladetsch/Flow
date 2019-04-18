<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

namespace Flow
{
    /// <inheritdoc cref="IFuture{T}" />
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITimedFuture<T> : IFuture<T>, ITimesOut
    {
    }

    public interface ITimedBarrier : IBarrier, ITimesOut
    {
    }

    public interface ITimedTrigger : ITrigger, ITimesOut
    {
    }

    public interface ITimedNode : INode, ITimesOut
    {
    }
}
