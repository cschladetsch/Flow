// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Collections.Generic;

namespace Flow
{
    public delegate void GroupHandler(IGroup node, ITransient child);

    /// <inheritdoc />
    /// <summary>
    /// A Group contains a collection of other Transients, and fires events when the contents of the group changes.
    /// <para>When a Group is stepped, nothing happens</para>
    /// <para>When a Group is resumed, all child generators are also resumed</para>
    /// <para>When a Group is suspended, all child generators are also suspended</para>
    /// </summary>
    public interface IGroup : IGenerator
    {
        IEnumerable<ITransient> Contents { get; }
        IEnumerable<IGenerator> Generators { get; }

        bool Empty { get; }

        // Occurs when a transient is added to this group.
        event GroupHandler Added;

        // Occurs when a transient is removed from this group.
        event GroupHandler Removed;

        // Add the specified transient to this group if it is not already a member of this group.
        void Add(IEnumerable<ITransient> trans);
        void Add(params ITransient[] trans);

        // Remove the specified transient from this group.
        void Remove(ITransient trans);
    }
}
