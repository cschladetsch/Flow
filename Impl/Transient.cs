// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    using System;

    /// <inheritdoc cref="ITransient" />
    public class Transient
        : Logger
        , ITransient
    {
        public event TransientHandler Completed
        {
            add
            {
                _completed += value;
                if (!Active)
                    value(this);
            }
            remove => _completed -= value;
        }

        public event TransientHandlerReason OnHowCompleted;

        public static bool DebugTrace;
        public static bool TimeoutsEnabled = true;

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

        public void Complete()
        {
            if (!Active)
                return;

            Active = false;

            _completed?.Invoke(this);
        }

        private TransientHandler _completed;

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
                Complete();
                return;
            }

            other.Completed += tr => CompletedBecause(other);
        }

        public ITransient Then(Action action)
            => Then(Factory.Do(action).AddTo(Kernel.Detail));

        public ITransient Then(Action<ITransient> action)
            => Then(Factory.Do(() => action(this)).AddTo(Kernel.Detail));

        public ITransient Then(IGenerator next)
        {
            if (next == null)
            {
                Warn("Cannot do nothing next.");
                return this;
            }

            if (!Active)
            {
                next.Resume();
                return this;
            }

            next.Suspend();

            void OnCompleted(ITransient self)
            {
                Completed -= OnCompleted;
                next.Resume();
            }

            Completed += OnCompleted;

            return this;
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

            Complete();
        }
    }
}

