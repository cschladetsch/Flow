using System;
using System.Security.AccessControl;

namespace Flow.Impl
{
    internal class TimedTrigger
        : Trigger
        , ITimedTrigger
    {
        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; internal set; }
        public bool HasTimedOut { get; protected set; }

        internal TimedTrigger(IKernel k, TimeSpan span)
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
            Timer.Elapsed -= HandleElapsed;
            HasTimedOut = true;

            Dispose();
        }
    }
}
