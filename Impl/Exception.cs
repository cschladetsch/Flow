// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    public class FutureNotSetException
        : Exception {
        public FutureNotSetException()
            : base("Future value not arrived yet.") {
        }
    }

    public class FutureAlreadySetException
        : Exception {
        public FutureAlreadySetException(string name)
            : base($"Future {name} already set.") {
        }
    }

    public class ReEntranceException
        : Exception {
        public ReEntranceException()
            : base("Method is not re-entrant.") {
        }
    }
}