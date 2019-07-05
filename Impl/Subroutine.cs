// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    using System;

    internal class Subroutine
        : Generator
        , ISubroutine
    {
        internal Action<IGenerator> Sub;

        public override void Step()
        {
            if (!(Active && Running))
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

    internal class Subroutine<TR>
        : Generator<TR>
        , ISubroutine<TR>
    {
        internal Func<IGenerator, TR> Sub;
        private readonly bool _once;

        internal Subroutine(bool once = false)
        {
            _once = once;
        }

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

            if (_once)
                Complete();
        }
    }
}
