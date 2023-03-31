// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    /// <inheritdoc cref="ITransient" />
    public class Transient
        : Logger
            , ITransient {
        protected static readonly bool TimeoutsEnabled = true;

        private TransientHandler _completed;
        public IFactory Factory => Kernel.Factory;
        public IFactory New => Factory;

        public event TransientHandler Completed {
            add {
                _completed += value;
                if (!Active) {
                    value(this);
                }
            }
            remove => _completed -= value;
        }

        public bool Active { get; protected set; } = true;
        public IKernel Kernel { get; /*internal*/ set; }

        public virtual string Name { get; set; }

        public ITransient Named(string name) {
            Name = name;
            return this;
        }

        public void Complete() {
            if (!Active) {
                return;
            }

            Active = false;

            _completed?.Invoke(this);
        }

        public ITransient AddTo(IGroup group) {
            group.Add(this);
            return this;
        }

        public event TransientHandlerReason OnHowCompleted;

        public override string ToString() {
            return Print.Object(this);
        }

        public void CompleteAfter(ITransient other) {
            if (!Active) {
                return;
            }

            if (other == null) {
                return;
            }

            if (!other.Active) {
                Complete();
                return;
            }

            other.Completed += tr => CompletedBecause(other);
        }

        public ITransient Then(Action action) {
            return Then(Factory.Do(action).AddTo(Kernel.Root));
        }

        public ITransient Then(Action<ITransient> action) {
            return Then(Factory.Do(() => action(this)).AddTo(Kernel.Root));
        }

        public ITransient Then(IGenerator next) {
            if (next == null) {
                Warn("Cannot do nothing next.");
                return this;
            }

            if (!Active) {
                next.Resume();
                return this;
            }

            next.Suspend();

            void OnCompleted(ITransient self) {
                Completed -= OnCompleted;
                next.Resume();
            }

            Completed += OnCompleted;

            return this;
        }

        public void CompleteAfter(TimeSpan span) {
            CompleteAfter(Factory.OneShotTimer(span));
        }

        public static bool IsNullOrInactive(ITransient other) {
            return other == null || !other.Active;
        }

        private void CompletedBecause(ITransient other) {
            if (!Active) {
                return;
            }

            OnHowCompleted?.Invoke(this, other);

            Complete();
        }
    }
}