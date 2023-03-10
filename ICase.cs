// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

namespace Flow {
    using System;

    public interface ICase<in T>
        where T
            : IComparable<T> {
        bool Matches(T val);

        IGenerator Body { get; }
    }
}

