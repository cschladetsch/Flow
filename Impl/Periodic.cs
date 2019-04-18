<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class Periodic
        : Subroutine<bool>
        , IPeriodic
=======
    internal class Periodic : Subroutine<bool>, IPeriodic
>>>>>>> 2156678... Updated to .Net4.5
    {
        public event TransientHandler Elapsed;

        public TimeSpan TimeRemaining => Kernel.Time.Now - TimeStarted;
        public DateTime TimeStarted { get; }
        public TimeSpan Interval { get; }

<<<<<<< HEAD
        private DateTime _expires;

=======
>>>>>>> 2156678... Updated to .Net4.5
        internal Periodic(IKernel kernel, TimeSpan interval)
        {
            Interval = interval;
            TimeStarted = kernel.Time.Now;
            _expires = TimeStarted + Interval;
            Sub = StepTimer;
        }

        private bool StepTimer(IGenerator self)
        {
            if (Kernel.Time.Now < _expires)
                return true;

            Elapsed?.Invoke(this);

            _expires = Kernel.Time.Now + Interval;

            return true;
        }
<<<<<<< HEAD
=======

        private DateTime _expires;
>>>>>>> 2156678... Updated to .Net4.5
    }
}
