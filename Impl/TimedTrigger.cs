using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class TimedTrigger
        : Trigger
        , ITimedTrigger
=======
    internal class TimedTrigger : Trigger, ITimedTrigger
>>>>>>> 2156678... Updated to .Net4.5
    {
        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; internal set; }
        public bool HasTimedOut { get; protected set; }
<<<<<<< HEAD
=======
        //public DateTime TimeOuTime { get { }}
>>>>>>> 2156678... Updated to .Net4.5

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
<<<<<<< HEAD
=======

        //private DateTime _started;
>>>>>>> 2156678... Updated to .Net4.5
    }
}
