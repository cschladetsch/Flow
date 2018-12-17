<<<<<<< HEAD
﻿using System.Collections.Generic;

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
=======
﻿namespace Flow.Impl
{
    internal class Sequence : Node, ISequence
    {
        internal Sequence()
        {
            _stepOne = true;
>>>>>>> 2156678... Updated to .Net4.5
        }
    }
}
