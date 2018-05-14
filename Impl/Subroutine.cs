// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    internal class Subroutine : Generator, ISubroutine
    {
        internal Action<IGenerator> Sub;

        public override void Step()
        {
            if (!Active || !Running)
                return;

            if (Sub == null)
            {
                Complete();
                return;
            }

            Sub(this);

            base.Step();
        }
    }

    internal class Subroutine<TR> : Generator<TR>, ISubroutine<TR>
    {
        internal Func<IGenerator, TR> Sub;

        public override void Step()
        {
            if (!Active || !Running)
                return;

            if (Sub == null)
            {
                Complete();
                return;
            }

            Value = Sub(this);

            base.Step();
        }
    }
}
