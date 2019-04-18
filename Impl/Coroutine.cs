// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    public class Coroutine
        : Generator
        , ICoroutine
    {
        public override object Value => _value;
        protected IEnumerator _state;
        private object _value;
        internal Func<IEnumerator> Start;

        public Coroutine()
        {
        }

        public Coroutine(Func<IEnumerator> start)
        {
            Start = start;
        }

        public override void Step()
        {
            if (!Running || !Active)
                return;

            if (_state == null)
            {
                if (Start == null)
                    CannotStart();
                else
                    _state = Start();

                if (_state == null)
                    CannotStart();
            }

            if (_state == null || !_state.MoveNext())
            {
                Complete();
                return;
            }

            _value = _state.Current;

            base.Step();
        }

        protected static void CannotStart()
        {
            throw new Exception("Coroutine cannot start");
        }
    }

    internal class Coroutine<T>
        : Generator<T>
        , ICoroutine<T>
    {
        internal Func<IEnumerator<T>> Start;
        private IEnumerator<T> _state;

        public override void Step()
        {
            if (!Running || !Active)
                return;

            if (_state == null)
            {
                if (Start == null)
                    CannotStart();
                else
                    _state = Start();

                if (_state == null)
                    CannotStart();
            }

            if (_state == null || !_state.MoveNext())
            {
                Complete();
                return;
            }

            Value = _state.Current;

            base.Step();
        }
    }
}
