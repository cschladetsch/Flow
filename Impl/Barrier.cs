<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections.Generic;
>>>>>>> 2156678... Updated to .Net4.5
using System.Linq;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class Barrier 
        : Group
        , IBarrier
    {
        internal Barrier()
        {
            if (Kernel.Log.Verbosity > 10)
                Completed += (tr) => Info($"Barrier {this} Completed");
=======
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
>>>>>>> 2156678... Updated to .Net4.5
        }

        public override void Post()
        {
            base.Post();

            if (Contents.Any(t => t.Active))
                return;

            if (Additions.Count == 0)
                Complete();
        }
<<<<<<< HEAD
    }
}
=======

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
>>>>>>> 2156678... Updated to .Net4.5
