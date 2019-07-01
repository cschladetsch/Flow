// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Linq;

namespace Flow.Impl
{
    internal class Barrier
        : Group
        , IBarrier
    {
        public override void Post()
        {
            base.Post();

            if (Contents.Any(t => t.Active))
                return;

            if (Additions.Count == 0)
                Dispose();
        }

        public new IBarrier AddTo(IGroup group) => this.AddToGroup<IBarrier>(group);
        public new IBarrier Named(string name) => this.SetName<IBarrier>(name);
    }
}

