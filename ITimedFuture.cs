// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

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
        new ITimedBarrier AddTo(IGroup group);
        new ITimedBarrier Named(string name);
    }

    public interface ITimedTrigger : ITrigger, ITimesOut
    {
    }

    public interface ITimedNode : INode, ITimesOut
    {
    }
}
