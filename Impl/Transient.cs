// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    public class Transient : ITransient
    {
        public event TransientHandler Completed;
        public event TransientHandlerReason WhyCompleted;

        public static bool DebugTrace;
        public bool Active { get; private set; }
        public IKernel Kernel { get; /*internal*/ set; }
        public IFactory Factory => Kernel.Factory;
        public IFactory New => Factory;

        public virtual string Name { get; set; }

        public Transient()
        {
            Active = true;
        }

        public override string ToString()
        {
            return Flow.Print.Object(this);
        }

        public ITransient SetName(string name)
        {
            Name = name;
            return this;
        }

        public void Complete()
        {
            if (!Active)
                return;

            Active = false;

            Completed?.Invoke(this);
        }

        public void CompleteAfter(ITransient other)
        {
            if (!Active)
                return;

            if (other == null)
                return;

            if (!other.Active)
            {
                Complete();
                return;
            }

            other.Completed += tr => CompletedBecause(other);
        }

        public void CompleteAfter(TimeSpan span)
        {
            CompleteAfter(Factory.OneShotTimer(span));
        }

        public static bool IsNullOrInactive(ITransient other)
        {
            return other == null || !other.Active;
        }

        private void CompletedBecause(ITransient other)
        {
            if (!Active)
                return;

            WhyCompleted?.Invoke(this, other);

            Complete();
        }
    }
}
