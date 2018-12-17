using System;

namespace Flow.Impl
{
    internal class TimedTrigger : Trigger, ITimedTrigger
    {
        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; internal set; }
        public bool HasTimedOut { get; protected set; }
        //public DateTime TimeOuTime { get { }}

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

            HasTimedOut = true;

            Complete();
        }

        //private DateTime _started;
    }
}
