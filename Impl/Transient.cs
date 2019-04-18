<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
    public class Transient :
        #if TRACE
        Logger,
        #endif
        ITransient
    {
        public event TransientHandler Completed;
        public event TransientHandlerReason WhyCompleted;

<<<<<<< HEAD
=======
        public static bool DebugTrace;
>>>>>>> 2156678... Updated to .Net4.5
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
            return Print.Object(this);
        }

        public ITransient Named(string name)
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
