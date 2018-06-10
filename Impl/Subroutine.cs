// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    internal class Subroutine : Generator, ISubroutine
    {
        internal Action<IGenerator> Sub;

        internal Subroutine()//bool once = false)
        {
            //_once = once;
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

            Sub(this);

            base.Step();

            //if (_once)
            //    Complete();
        }

        //private readonly bool _once;
    }

    internal class Subroutine<TR> : Generator<TR>, ISubroutine<TR>
    {
        internal Func<IGenerator, TR> Sub;

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
        private readonly bool _once;
    }
}
