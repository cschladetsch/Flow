// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    internal class Timer
        : Periodic
        , ITimer
    {
        public DateTime TimeEnds { get; private set; }

        private void TimedOutHandler(ITransient sender) => Complete();

        /// <summary>
        /// Timer based on game time, not real time.
        /// </summary>
        internal Timer(IKernel kernel, TimeSpan span)
            : base(kernel, span)
        {
            TimeEnds = kernel.Time.Now + span;
            Elapsed += TimedOutHandler;
        }
    }
}
