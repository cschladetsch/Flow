using System;


namespace Flow.Impl
{
    public class Case<T> : Generator<IGenerator>, ICase<T> where T : IComparable<T>
    {
        T _compare;

        public Case(T val, IGenerator gen)
        {
            Value = gen;
            _compare = val;
        }

        public bool Matches(T val)
        {
            return val.CompareTo(_compare) == 0;// ? Value : null;
        }
    }
}
