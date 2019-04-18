using System.Collections.Generic;

namespace Flow.Impl
{
    /// <inheritdoc cref="ISequence"/>
    internal class Sequence
        : Node
        , ISequence
    {
        internal Sequence()
        {
            _StepOne = true;
        }

        internal Sequence(IEnumerable<IGenerator> gens)
            : this()
        {
            Add(gens);
        }
    }
}
