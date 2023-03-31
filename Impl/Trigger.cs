// (C) 2012 christian.schladetsch@gmail.com. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow.Impl {
    internal class Trigger
        : Group
            , ITrigger {
        internal Trigger() {
            OnRemoved += Trip;
        }

        public event Action<ITrigger, ITransient> OnTripped;

        public ITransient Reason { get; private set; }

        private void Trip(IGroup self, ITransient other) {
            Reason = other;

            OnTripped?.Invoke(this, other);

            Complete();
        }
    }
}