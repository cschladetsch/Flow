// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Collections.Generic;

namespace Flow
{
    /// <inheritdoc />
    /// <summary>
    /// A channel is a buffered input/output stream.
    /// <para>If a channel is created with a TypedGenerator, that is used as the source of the channel</para>
    /// </summary>
    public interface IChannel<T> : ITransient
    {
        void Insert(T val);
        IFuture<T> Extract();
        List<T> ExtractAll();
        void Flush();
    }
}
