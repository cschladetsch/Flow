// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    internal class TimedFuture<T>
        : Future<T>
            , ITimedFuture<T> {
        internal TimedFuture(IKernel k, TimeSpan span) {
            Timer = k.Factory.OneShotTimer(span);
            if (TimeoutsEnabled) {
                Timer.Elapsed += HandleElapsed;
            }

            //Completed += tr => Timer.Complete();
        }

        public ITimedFuture<T> Then(Action<ITimedFuture<T>> action) {
            return Then(() => action(this)) as ITimedFuture<T>;
        }

        public event TimedOutHandler TimedOut {
            add {
                _timedOut += value;
                if (HasTimedOut) {
                    value(this);
                }
            }
            remove => _timedOut -= value;
        }

        public bool HasTimedOut { get; protected set; }

        public ITimer Timer { get; internal set; }

        private event TimedOutHandler _timedOut;

        private void HandleElapsed(ITransient sender) {
            if (!Active) {
                return;
            }

            _timedOut?.Invoke(this);

            HasTimedOut = true;

            Complete();
        }
    }
}