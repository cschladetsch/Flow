// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl {
    using System;

    internal class Trigger
        : Group
        , ITrigger {
        public event Action<ITrigger, ITransient> OnTripped;

        public ITransient Reason { get; private set; }

        internal Trigger()
            => OnRemoved += Trip;

        private void Trip(IGroup self, ITransient other) {
            Reason = other;

            OnTripped?.Invoke(this, other);

            Complete();
        }
    }
}

