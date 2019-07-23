// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    internal class Periodic
        : Subroutine<bool>
        , IPeriodic
    {
        public event TransientHandler Elapsed;

        public TimeSpan TimeRemaining => Kernel.Time.Now - TimeStarted;
        public DateTime TimeStarted { get; }
        public TimeSpan Interval { get; }

        private DateTime _expires;

        internal Periodic(IKernel kernel, TimeSpan interval)
        {
            Interval = interval;
            TimeStarted = kernel.Time.Now;
            kernel.Detail.Add(this);
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
    }
}
