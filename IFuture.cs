// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    /// <inheritdoc />
    /// <summary>
    /// When its' Value property is first set, set Available property to true, and Complete.
    /// </summary>
    public interface IFuture<T>
        : ITransient
    {
        bool Available { get; }
        T Value { get; set; }
    }
}

