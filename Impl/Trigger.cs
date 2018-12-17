// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    internal class Trigger : Group, ITrigger
    {
        public event TriggerHandler Tripped;
        public ITransient Reason { get; private set; }

        internal Trigger()
        {
            Removed += Trip;
        }

        private void Trip(IGroup self, ITransient other)
        {
            Reason = other;

            Tripped?.Invoke(this, other);

            Complete();
        }
    }
}
