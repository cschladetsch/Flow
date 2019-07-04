using System;
using System.Collections.Generic;

namespace Flow.Impl
{
    /// <summary>
    /// A barrier that completes after a certain time.
    /// Barriers normally only complete when all of it contents are complete.
    /// </summary>
    internal class TimedBarrier
        : Barrier
        , ITimedBarrier
    {
        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; }
        public bool HasTimedOut { get; set; }

        public TimedBarrier(IKernel kernel, TimeSpan span, IEnumerable<ITransient> contents)
        {
            Timer = kernel.Factory.OneShotTimer(span);
            if (TimeoutsEnabled)
                Timer.Elapsed += Elapsed;

            foreach (var tr in contents)
                Add(tr);
        }

        private void Elapsed(ITransient tr)
        {
            Timer.Elapsed -= Elapsed;
            HasTimedOut = true;
            TimedOut?.Invoke(this);
            Dispose();
        }

        public new ITimedBarrier AddTo(IGroup group) => this.AddToGroup<ITimedBarrier>(group);
        public new ITimedBarrier Named(string name) => this.SetName<ITimedBarrier>(name);
    }
}
