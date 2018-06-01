// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Impl
{
    internal class Barrier : Group, IBarrier
    {
        internal Barrier()
        {
            Added += (barrier, tr) => _list.Add(tr);
            Completed += IDided;
        }

        void IDided(ITransient tr)
        {
            Info($"Barrier {this} dead");
        }

        public override void Post()
        {
            base.Post();

            if (Contents.Any(t => t.Active))
                return;

            if (Additions.Count == 0)
                Complete();
        }

        public IBarrier ForEach<T>(Action<T> act) where T : class
        {
            Completed += tr =>
            {
                foreach (var item in _list.Cast<T>())
                    act(item);
            };

            return this;
        }

        private readonly List<ITransient> _list = new List<ITransient>();
    }
}
