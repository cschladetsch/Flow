// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    internal class Timer
        : Periodic
            , ITimer {
        /// <summary>
        ///     Timer based on game time, not real time.
        /// </summary>
        internal Timer(IKernel kernel, TimeSpan span)
            : base(kernel, span) {
            TimeEnds = kernel.Time.Now + span;
            Elapsed += TimedOutHandler;
        }

        public DateTime TimeEnds { get; }

        private void TimedOutHandler(ITransient sender) {
            Complete();
        }
    }
}