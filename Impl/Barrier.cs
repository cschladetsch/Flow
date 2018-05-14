// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Linq;

namespace Flow.Impl
{
    internal class Barrier : Group, IBarrier
    {
        public override void Post()
        {
            base.Post();

            if (Contents.Any(t => t.Active))
                return;

            if (Additions.Count == 0)
                Complete();
        }
    }
}
