// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    internal class Future<T>
        : Transient
        , IFuture<T>
    {
        public event FutureHandler<T> Arrived
        {
            add
            {
                _arrived += value;
                if (Available)
                    value(this);
            }
            remove => _arrived -= value;
        }

        public bool Available { get; private set; }

        public T Value
        {
            get
            {
                if (!Available)
                    throw new FutureNotSetException();

                return _value;
            }
            set
            {
                if (Available)
                    throw new FutureAlreadySetException(Name);

                _value = value;
                Available = true;

                _arrived?.Invoke(this);

                Complete();
            }
        }

        public IFuture<T> Then(Action<IFuture<T>> action)
            => Then(() => action(this)) as IFuture<T>;

        private T _value;
        private FutureHandler<T> _arrived;
    }
}

