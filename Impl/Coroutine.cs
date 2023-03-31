// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Flow.Impl {
    public class Coroutine
        : Generator
            , ICoroutine {
        protected IEnumerator _state;
        private object _value;
        internal Func<IEnumerator> Start;

        public Coroutine() {
        }

        public Coroutine(Func<IEnumerator> start)
            : this() {
            Start = start;
        }

        public override object Value => _value;

        public override void Step() {
            if (!Running || !Active) {
                return;
            }

            if (_state == null) {
                if (Start == null) {
                    CannotStart();
                }
                else {
                    _state = Start();
                }

                if (_state == null) {
                    CannotStart();
                }
            }

            if (_state == null || !_state.MoveNext()) {
                Complete();
                return;
            }

            _value = _state.Current;

            base.Step();
        }

        protected static void CannotStart() {
            throw new Exception("Coroutine cannot start");
        }
    }

    internal class Coroutine<T>
        : Generator<T>
            , ICoroutine<T> {
        private IEnumerator<T> _state;
        internal Func<IEnumerator<T>> Start;

        public override void Step() {
            if (!Running || !Active) {
                return;
            }

            if (_state == null) {
                if (Start == null) {
                    CannotStart();
                }
                else {
                    _state = Start();
                }

                if (_state == null) {
                    CannotStart();
                }
            }

            if (_state == null || !_state.MoveNext()) {
                Complete();
                return;
            }

            Value = _state.Current;

            base.Step();
        }
    }
}