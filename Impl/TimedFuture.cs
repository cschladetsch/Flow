// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    using System;

    internal class TimedFuture<T>
        : Future<T>
        , ITimedFuture<T>
    {
        public ITimedFuture<T> Then(Action<ITimedFuture<T>> action)
            => Then(Factory.Do(() => action(this)).AddTo(Kernel.Detail)) as ITimedFuture<T>;

        public event TimedOutHandler TimedOut
        {
            add
            {
                _timedOut += value;
                if (HasTimedOut)
                    value(this);
            }
            remove => _timedOut -= value;
        }

        public bool HasTimedOut { get; protected set; }

        public ITimer Timer { get; internal set; }

        private event TimedOutHandler _timedOut;

        internal TimedFuture(IKernel k, TimeSpan span)
        {
            Timer = k.Factory.OneShotTimer(span);
            if (TimeoutsEnabled)
                Timer.Elapsed += HandleElapsed;

            //Completed += tr => Timer.Complete();
        }

        private void HandleElapsed(ITransient sender)
        {
            if (!Active)
                return;

            _timedOut?.Invoke(this);

            HasTimedOut = true;

            Complete();
        }
    }
}
