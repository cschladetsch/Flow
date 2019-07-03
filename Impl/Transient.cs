// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    using System;

    /// <inheritdoc cref="ITransient" />
    public class Transient
        : Logger
        , ITransient
    {
        public event TransientHandler OnDisposed;
        public event TransientHandlerReason OnHowCompleted;

        public static bool DebugTrace;

        public bool Active { get; private set; } = true;
        public IKernel Kernel { get; /*internal*/ set; }
        public IFactory Factory => Kernel.Factory;
        public IFactory New => Factory;

        public virtual string Name { get; set; }

        public override string ToString()
            => Print.Object(this);

        public ITransient Named(string name)
        {
            Name = name;
            return this;
        }

        public void Dispose()
        {
            if (!Active)
                return;

            Active = false;

            OnDisposed?.Invoke(this);
        }

        public ITransient AddTo(IGroup group)
        {
            group.Add(this);
            return this;
        }

        public void CompleteAfter(ITransient other)
        {
            if (!Active)
                return;

            if (other == null)
                return;

            if (!other.Active)
            {
                Dispose();
                return;
            }

            other.OnDisposed += tr => CompletedBecause(other);
        }

        public void CompleteAfter(TimeSpan span)
            => CompleteAfter(Factory.OneShotTimer(span));

        public static bool IsNullOrInactive(ITransient other)
            => other == null || !other.Active;

        private void CompletedBecause(ITransient other)
        {
            if (!Active)
                return;

            OnHowCompleted?.Invoke(this, other);

            Dispose();
        }
    }
}

