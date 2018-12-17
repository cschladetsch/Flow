<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class TimedFuture<T>
        : Future<T>
        , ITimedFuture<T>
=======
    internal class TimedFuture<T> : Future<T>, ITimedFuture<T>
>>>>>>> 2156678... Updated to .Net4.5
    {
        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; internal set; }
        public bool HasTimedOut { get; protected set; }

        internal TimedFuture(IKernel k, TimeSpan span)
        {
            Timer = k.Factory.OneShotTimer(span);
            k.Root.Add(Timer);
            Timer.Elapsed += HandleElapsed;
        }

        private void HandleElapsed(ITransient sender)
        {
            if (!Active)
                return;

            TimedOut?.Invoke(this);

            HasTimedOut = true;

            Complete();
        }
    }
}
