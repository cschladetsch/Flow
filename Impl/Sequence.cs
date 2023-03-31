// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Collections.Generic;
using System.Linq;

namespace Flow.Impl {
    /// <inheritdoc cref="ISequence" />
    internal class Sequence
        : Node
            , ISequence {
        internal Sequence() {
            _StepOne = true;
        }

        internal Sequence(IEnumerable<IGenerator> gens)
            : this() {
            Add(gens);
        }

        public override void Post() {
            if (!Contents.Any()) {
                Complete();
            }
        }
    }
}