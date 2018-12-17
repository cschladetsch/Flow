// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    public delegate void FutureHandler<T>(IFuture<T> future);

    /// <inheritdoc />
    /// <summary>
    /// When its Value property is first set, a Future fires its Arrived event, sets its Available property to true, and
    /// Completes itself.
    /// </summary>
    public interface IFuture<T> : ITransient
    {
        bool Available { get; }
        T Value { get; set; }
        event FutureHandler<T> Arrived;
    }
}
