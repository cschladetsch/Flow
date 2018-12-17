// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    public interface ICase<in T> where T : IComparable<T>
    {
        bool Matches(T val);

        IGenerator Body { get; }
    }
}
