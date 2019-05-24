// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    public delegate void FutureHandler<T>(IFuture<T> future);

    /// <inheritdoc />
    /// <summary>
    /// When its Value property is first set, a Future fires its Arrived event, sets its Available property to true, and
    /// Completes itself.
    /// </summary>
    [Obsolete("Are you sure you don't want use a ITimedFuture?")]
    public interface IFuture<T>
        : ITransient
    {
        bool Available { get; }
        T Value { get; set; }
        event FutureHandler<T> Arrived;
    }
}
