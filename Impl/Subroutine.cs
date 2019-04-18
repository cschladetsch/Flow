<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class Subroutine
        : Generator
        , ISubroutine
=======
    internal class Subroutine : Generator, ISubroutine
>>>>>>> 2156678... Updated to .Net4.5
    {
        internal Action<IGenerator> Sub;

        public override void Step()
        {
<<<<<<< HEAD
            if (!(Active && Running))
=======
            if (!Active || !Running)
>>>>>>> 2156678... Updated to .Net4.5
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

<<<<<<< HEAD
    internal class Subroutine<TR>
        : Generator<TR>
        , ISubroutine<TR>
    {
        internal Func<IGenerator, TR> Sub;
        private readonly bool _once;
=======
    internal class Subroutine<TR> : Generator<TR>, ISubroutine<TR>
    {
        internal Func<IGenerator, TR> Sub;
>>>>>>> 2156678... Updated to .Net4.5

        internal Subroutine(bool once = false)
        {
            _once = once;
        }
<<<<<<< HEAD

=======
>>>>>>> 2156678... Updated to .Net4.5
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
<<<<<<< HEAD
=======
        private readonly bool _once;
>>>>>>> 2156678... Updated to .Net4.5
    }
}
