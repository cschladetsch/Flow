// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    internal class TimedTrigger
        : Trigger
            , ITimedTrigger {
        internal TimedTrigger(IKernel k, TimeSpan span) {
            Timer = k.Factory.OneShotTimer(span);
            k.Root.Add(Timer);

            if (TimeoutsEnabled) {
                Timer.Elapsed += HandleElapsed;
            }
        }

        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; internal set; }
        public bool HasTimedOut { get; protected set; }

        private void HandleElapsed(ITransient sender) {
            if (!Active) {
                return;
            }

            TimedOut?.Invoke(this);
            Timer.Elapsed -= HandleElapsed;
            HasTimedOut = true;

            Complete();
        }
    }
}