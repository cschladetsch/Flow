// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections.Generic;

namespace Flow.Impl {
    /// <summary>
    ///     A barrier that completes after a certain time.
    ///     Barriers normally only complete when all of it contents are complete.
    /// </summary>
    internal class TimedBarrier
        : Barrier
            , ITimedBarrier {
        public TimedBarrier(IKernel kernel, TimeSpan span, IEnumerable<ITransient> contents) {
            Timer = kernel.Factory.OneShotTimer(span);
            if (TimeoutsEnabled) {
                Timer.Elapsed += Elapsed;
            }

            foreach (var tr in contents)
                Add(tr);
        }

        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; }
        public bool HasTimedOut { get; set; }

        public new ITimedBarrier AddTo(IGroup group) {
            return this.AddToGroup<ITimedBarrier>(group);
        }

        public new ITimedBarrier Named(string name) {
            return this.SetName<ITimedBarrier>(name);
        }

        private void Elapsed(ITransient tr) {
            Timer.Elapsed -= Elapsed;
            HasTimedOut = true;
            TimedOut?.Invoke(this);
            Complete();
        }
    }
}