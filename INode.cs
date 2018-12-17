// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{

    /// <inheritdoc />
    /// <summary>
    /// A Node is a Group that steps all referenced Generators when it itself is Stepped, and similarly for Pre and Post.
    /// </summary>
    public interface INode : IGroup
    {
        void Add(params IGenerator[] gens);
    }

    /// <inheritdoc />
    /// <summary>
    /// A Sequence is a node that steps only the first generator until it is completed, then moves
    /// on to the next. When the sequence is empty it Completes.
    /// </summary>
    public interface ISequence : INode
    {
    }
}
