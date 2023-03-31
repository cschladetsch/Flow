// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    internal class Case<T>
        : ICase<T> where T
        : IComparable<T> {
        private readonly T _compare;

        internal Case(T val, IGenerator gen) {
            _compare = val;
            Body = gen;
        }

        public IGenerator Body { get; }

        public bool Matches(T val) {
            return val.CompareTo(_compare) == 0;
        }
    }
}