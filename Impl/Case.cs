// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl
{
    internal class Case<T>
        : ICase<T> where T 
            : IComparable<T>
    {
        public IGenerator Body { get; }

        private readonly T _compare;

        internal Case(T val, IGenerator gen)
        {
            _compare = val;
            Body = gen;
        }

        public bool Matches(T val)
        {
            return val.CompareTo(_compare) == 0;
        }
    }
}
