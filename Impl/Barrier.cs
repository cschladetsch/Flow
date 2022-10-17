// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl {
    using System.Linq;

    internal class Barrier
        : Group
        , IBarrier {
        public override void Post() {
            base.Post();

            if (Contents.Any(t => t.Active))
                return;

            if (_Additions.Count == 0)
                Complete();
        }

        public new IBarrier AddTo(IGroup group)
            => this.AddToGroup<IBarrier>(group);

        public new IBarrier Named(string name)
            => this.SetName<IBarrier>(name);
    }
}

